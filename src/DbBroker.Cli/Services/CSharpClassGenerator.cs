using System.Diagnostics;
using System.Text;
using DbBroker.Cli.Commands.Sync;
using DbBroker.Cli.Exceptions;
using DbBroker.Cli.Extensions;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services;

public class CSharpClassGenerator : ICSharpClassGenerator
{
    public async Task<int> GenerateAsync(DbBrokerConfigContext context)
    {
        if (!string.IsNullOrEmpty(SyncCommand.Options.Context) && !SyncCommand.Options.Context.Equals(context.Name, StringComparison.InvariantCultureIgnoreCase))
        {
            "Context ignored.".Warning(context.Namespace);
            return 0;
        }

        "Retrieving database metadata...".Log(context.Namespace);

        try
        {
            if (context.Provider is null)
            {
                throw new DbBrokerConfigurationException("The context does not have a Provider specified. ");
            }

            if (context.ConnectionString is null)
            {
                throw new DbBrokerConfigurationException("The context does not have a Connection String specified. ");
            }
            
            using var connection = context.GetDbConnection();
            connection.Open();

            var metadataProvider = context.GetMetadataProvider();

            var tablesDescriptors = await metadataProvider
                .GetTableDescriptorsAsync(connection, context);

            var viewsDescriptors = await metadataProvider
                .GetViewsDescriptorsAsync(connection, context);

            var outputDirectory = (context.Namespace?.Split(".")?.Length > 1 ? string.Join(Path.DirectorySeparatorChar, context.Namespace.Split(".").Skip(1)) : context.Namespace) ?? string.Empty;
            outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), outputDirectory!);

            var resolvedOutputDirectory = context.OutputDirectory ?? outputDirectory;

            if (context.ClearOutputDirectory)
            {
                try
                {
                    Directory.Delete(resolvedOutputDirectory, true);
                }
                catch (Exception ex)
                {
                    ex.Message.Warning(context.Namespace);
                }
            }

            Directory.CreateDirectory(resolvedOutputDirectory);

            $"Output directory: {resolvedOutputDirectory}".Log(context.Namespace);

            var sqlTransformer = context.GetSqlTransformer();
            var providerDefaultConfig = context.GetDefaultProviderConfig();

            await Task.WhenAll(
                GenerateClassesForTables(tablesDescriptors, context, sqlTransformer, providerDefaultConfig, resolvedOutputDirectory),
                GenerateClassesForViews(viewsDescriptors, context, sqlTransformer, resolvedOutputDirectory));
        }
        catch (DbBrokerConfigurationException configEx)
        {
            configEx.Message.Error(context.Namespace);

            if (Debugger.IsAttached)
            {
                configEx.StackTrace?.Error(context.Namespace);
            }

            SyncCommand.Results.Add(context.Namespace, ExitCodes.CONFIG_FILE_INIT_ERROR);
            return ExitCodes.CONFIG_FILE_INIT_ERROR;
        }
        catch (Exception ex)
        {
            ex.Message.Error(context.Namespace);

            if (Debugger.IsAttached)
            {
                ex.StackTrace?.Error(context.Namespace);
            }

            SyncCommand.Results.Add(context.Namespace, ExitCodes.CONTEXT_GENERIC_ERROR);
            return 1;
        }

        return 0;
    }

    static async Task GenerateClassesForTables(
        Dictionary<string, TableDescriptorModel> tablesDescriptors,
        DbBrokerConfigContext context,
        ISqlTransformer sqlTransformer,
        IProviderDefaultConfiguration providerDefaultConfig,
        string outputDirectory)
    {
        $"{tablesDescriptors.Count} tables found.".Log(context.Namespace);

        if (tablesDescriptors.Count == 0)
        {
            "Make sure the user provided has SELECT permission on metadata tables.".Warning(context.Namespace, 0);
        }

        var allKeys = tablesDescriptors
            .Select(tableDescriptor => tableDescriptor.Value.Keys)
            .SelectMany(x => x);

        StringBuilder propsString = new();
        StringBuilder refsString = new();
        StringBuilder collectionsString = new();
        foreach (var tableDescriptor in tablesDescriptors)
        {
            propsString.Clear();
            refsString.Clear();
            collectionsString.Clear();

            // Find out all references to this table
            var referencesTo = allKeys.Where(x => x.ReferencedTable.Equals(tableDescriptor.Value.TableName));
            if (referencesTo.Any())
            {
                foreach (var reference in referencesTo)
                {
                    if (!tablesDescriptors.TryGetValue(reference.TableFullName, out TableDescriptorModel? tableDescriptorModel))
                    {
                        throw new InvalidOperationException($"The '{reference.TableName}' table is not listed in this context. If you are filtering the tables make sure to list all referenced tables in all levels.");
                    }

                    var primaryKeyColumnName = tableDescriptorModel
                        .Keys?
                        .FirstOrDefault(x => x.ConstraintType.Equals("PrimaryKey"))?
                        .ColumnName;

                    if (primaryKeyColumnName is not null)
                    {
                        collectionsString.AppendLine(
                            Constants.EDM_COLLECTION_REFERENCE_TEMPLATE
                                .Replace("$$PROPERTYNAME$$", $"{reference.TableName.ToCamelCase()}{reference.ColumnName.ToCamelCase()}")
                                .Replace("$$SCHEMANAME$$", reference.SchemaName)
                                .Replace("$$TABLENAME$$", reference.ReferencedTable)
                                .Replace("$$COLUMNNAME$$", reference.ReferencedColumn)
                                .Replace("$$REFCOLUMNNAME$$", reference.ColumnName)
                                .Replace("$$REFSCHEMANAME$$", reference.SchemaName)
                                .Replace("$$REFTABLENAME$$", reference.TableName)
                                .Replace("$$PKCOLUMNNAME$$", primaryKeyColumnName)
                                .Replace("$$REFTYPENAME$$", $"{context.ModelsPrefix}{reference.TableName.ToCamelCase()}{context.ModelsSufix}")
                        );
                    }
                }
            }

            foreach (var item in tableDescriptor.Value.Columns)
            {
                if (item is null)
                {
                    continue;
                }

                var isPrimaryKey = tableDescriptor
                    .Value
                    .Keys
                    .Any(x => x.KeyFullName.Equals(item.ColumnFullName) && x.ConstraintType.Equals("PrimaryKey"));

                propsString.AppendLine(
                    Constants.EDM_PROPERTY_TEMPLATE
                        .Replace("$TYPE", sqlTransformer.GetCSharpType(item.DataType, item.MaxLength ?? "250", item.DataTypePrecision, item.DataTypeScale, item.IsNullable))
                        .Replace("$KEY", isPrimaryKey ? "Key, " : string.Empty)
                        .Replace("$COLUMN_NAME", item?.ColumnName)
                        .Replace("$$PROVIDER_DBTYPE$$", context.Provider!.GetDbTypeString(item!))
                        .Replace("$NAME", item?.ColumnName.ToCamelCase()));

                var foreignKey = tableDescriptor
                    .Value
                    .Keys
                    .FirstOrDefault(x => x.ColumnName.Equals(item?.ColumnName) && x.ConstraintType.Equals("ForeignKey"));

                if (foreignKey is not null)
                {
                    if (!tablesDescriptors.TryGetValue($"{foreignKey.SchemaName}.{foreignKey.ReferencedTable}", out TableDescriptorModel? foreignKeyTableDescriptorModel))
                    {
                        throw new InvalidOperationException($"The '{foreignKey.ReferencedTable}' table is not listed in this context. If you are filtering the tables make sure to list all referenced tables in all levels.");
                    }

                    refsString.AppendLine(
                        Constants.EDM_REFERENCE_TEMPLATE
                            .Replace("$$PROPERTYNAME$$", item?.ColumnName.ToCamelCase())
                            .Replace("$$SCHEMANAME$$", item?.SchemaName)
                            .Replace("$$TABLENAME$$", item?.TableName)
                            .Replace("$$COLUMNNAME$$", item?.ColumnName)
                            .Replace("$$COLUMNALLOWNULLS$$", item?.IsNullable.ToString().ToLowerInvariant())
                            .Replace("$$REFCOLUMNNAME$$", foreignKey.ReferencedColumn)
                            .Replace("$$REFSCHEMANAME$$", foreignKey.SchemaName)
                            .Replace("$$REFTABLENAME$$", foreignKey.ReferencedTable)
                            .Replace("$$PKCOLUMNNAME$$", foreignKey.ReferencedColumn)
                            .Replace("$$REFTYPENAME$$", $"{context.ModelsPrefix}{foreignKeyTableDescriptorModel.TableName.ToCamelCase()}{context.ModelsSufix}")
                    );
                }
            }

            var configContextTable = context.Tables.FirstOrDefault(x => x.Name.Equals(tableDescriptor.Value.TableName));

            await File.WriteAllTextAsync(
                Path.Combine(outputDirectory, $"{context.ModelsPrefix}{tableDescriptor.Value.TableName.ToCamelCase()}{context.ModelsSufix}.cs"),
                Constants.EDM_CLASS_TEMPLATE
                    .Replace("$$PROVIDER_CLIENT_NAMESPACE$$", context.Provider!.GetProviderClientUsingString())
                    .Replace("$NAMESPACE", context.Namespace ?? "-")
                    .Replace("$CLASSNAME", $"{context.ModelsPrefix}{tableDescriptor.Value.TableName.ToCamelCase()}{context.ModelsSufix}")
                    .Replace("$TABLE", tableDescriptor.Value.TableName)
                    .Replace("$SCHEMA", tableDescriptor.Value.SchemaName)
                    .Replace("$PROPERTIES", propsString.ToString())
                    .Replace("$REFERENCES", refsString.ToString())
                    .Replace("$COLLECTIONS", collectionsString.ToString())
                    .Replace("$PROVIDER", context.Provider.ToString())
                    .Replace("$ISQLINSERTTEMPLATETYPEFULLNAME",
                        configContextTable?.SqlInsertTemplateTypeFullName
                        ?? context.DefaultSqlInsertTemplateTypeFullName
                        ?? providerDefaultConfig.ISqlInsertTemplateTypeFullName)
                    .Replace("$ISQLINSERTTEMPLATETYPEARGUMENTS",
                        string.Join(",", configContextTable?.SqlInsertTemplateArguments?.Values.Select(x => $"\"{x}\"") ?? [])
                    ));
        }
    }

    /// <summary>
    /// By default database View Data Models are created as a representation of a database view tuple.
    /// <para>If the 'SplitsOn' option is configured, then: 
    /// (1) Classes will be created inside of the View Data Model to represent the entities present on the database View, 
    /// (2) The View Data Model will have properties to load the data for these entities: instances or collections,
    /// (3) A 'tuple' class will be created inside of the View Data Model to help with writing operations.
    /// </para>
    /// </summary>
    static async Task GenerateClassesForViews(
        Dictionary<string, ViewDescriptorModel> viewsDescriptors,
        DbBrokerConfigContext context,
        ISqlTransformer sqlTransformer,
        string outputDirectory)
    {
        $"{viewsDescriptors.Count} views found.".Log(context.Namespace);

        if (viewsDescriptors.Count > 0)
        {
            Directory.CreateDirectory(Path.Combine(context.OutputDirectory ?? outputDirectory, "Views"));
        }

        StringBuilder propsString = new();
        StringBuilder refsString = new();
        StringBuilder collectionsString = new();
        StringBuilder classesString = new();
        StringBuilder tupleString = new();
        StringBuilder tuplePropertiesString = new();
        foreach (var viewDescriptor in viewsDescriptors)
        {
            propsString.Clear();
            refsString.Clear();
            collectionsString.Clear();
            classesString.Clear();
            tupleString.Clear();
            tuplePropertiesString.Clear();

            var viewConfig = context
                .Views
                .FirstOrDefault(x => x.Name == viewDescriptor.Value.ViewFullName || x.Name == viewDescriptor.Value.ViewName);

            // Check if the splitOn column is specified
            var splitsOn = viewConfig?.SplitsOn;

            if (splitsOn is null)
            {
                var index = 0;
                foreach (var item in viewDescriptor.Value.Columns)
                {
                    propsString.AppendLine(
                        Constants.EDM_PROPERTY_TEMPLATE
                            .Replace("$KEY", index == 0 ? "Key, " : string.Empty)
                            .Replace("$COLUMN_NAME", item.ColumnName)
                            .Replace("$$PROVIDER_DBTYPE$$", context.Provider.GetDbTypeString(item!))
                            .Replace("$TYPE", sqlTransformer.GetCSharpType(item.DataType, item?.MaxLength ?? "250", item?.DataTypePrecision, item?.DataTypeScale, item?.IsNullable ?? false))
                            .Replace("$NAME", item?.ColumnName.ToCamelCase()));
                    index++;
                }
            }

            if (splitsOn is not null)
            {
                // first entity
                DbBrokerConfigContextViewSplitOnItem currentSplitOn = new()
                {
                    Column = viewDescriptor.Value.Columns.First().ColumnName,
                    Type = viewConfig?.TypeName ?? $"{context.ModelsPrefix}{viewDescriptor.Value.ViewName.ToCamelCase()}{context.ModelsSufix}",
                    Collection = false
                };

                var entities = new Dictionary<string, ViewSplittedEntityModel>
                {
                    { currentSplitOn.Type, new ViewSplittedEntityModel(currentSplitOn) }
                };

                var index = 0;
                foreach (var item in viewDescriptor.Value.Columns)
                {
                    // check if next the entity
                    var nextEntity = splitsOn.FirstOrDefault(x => x.Column == item.ColumnName);
                    if (nextEntity is not null)
                    {
                        currentSplitOn = nextEntity;
                        entities[currentSplitOn.Type] = new ViewSplittedEntityModel(currentSplitOn);
                        index = 0;
                    }

                    item.Index = index;
                    entities[currentSplitOn.Type].Columns.Add(item);
                    index++;
                }

                // properties
                foreach (var item in entities.Values.First().Columns)
                {
                    propsString.AppendLine(
                        Constants.EDM_VIEW_PROPERTY_TEMPLATE
                            .Replace("$KEY", item.Index == 0 ? "Key, " : string.Empty)
                            .Replace("$COLUMN_NAME", item.ColumnName)
                            .Replace("$$PROVIDER_DBTYPE$$", context.Provider.GetDbTypeString(item!))
                            .Replace("$TYPE", sqlTransformer.GetCSharpType(item.DataType, item?.MaxLength ?? "250", item?.DataTypePrecision, item?.DataTypeScale, item?.IsNullable ?? false))
                            .Replace("$NAME", item?.ColumnName.ToCamelCase()));

                    tuplePropertiesString.AppendLine(
                        Constants.EDM_PROPERTY_INDENTED_TEMPLATE
                            .Replace("$KEY", item!.Index == 0 ? "Key, " : string.Empty)
                            .Replace("$COLUMN_NAME", item.ColumnName)
                            .Replace("$$PROVIDER_DBTYPE$$", context.Provider.GetDbTypeString(item!))
                            .Replace("$TYPE", sqlTransformer.GetCSharpType(item.DataType, item?.MaxLength ?? "250", item!.DataTypePrecision, item!.DataTypeScale, item!.IsNullable))
                            .Replace("$NAME", item!.ColumnName.ToCamelCase()));
                }

                // references and collections
                foreach (var entity in entities.Values.Skip(1))
                {
                    refsString.AppendLine(
                        (entity.SplitOnItem.Collection ? Constants.EDM_COLLECTION_REFERENCE_VIEW_TEMPLATE : Constants.EDM_REFERENCE_VIEW_TEMPLATE)
                            .Replace("$$REFTYPENAME$$", entity.SplitOnItem.Type)
                            .Replace("$$REFPROPERTYNAME$$", entity.SplitOnItem.Column.ToCamelCase()));

                    // classes properties/class
                    var classProperties = string.Join(
                        Environment.NewLine,
                        entity.Columns.Select(c => Constants.EDM_VIEW_PROPERTY_INDENTED_TEMPLATE
                            .Replace("$KEY", c.Index == 0 ? "Key, " : string.Empty)
                            .Replace("$COLUMN_NAME", c.ColumnName)
                            .Replace("$TYPE", sqlTransformer.GetCSharpType(c.DataType, c?.MaxLength ?? "250", c?.DataTypePrecision, c?.DataTypeScale, c?.IsNullable ?? false))
                            .Replace("$$PROVIDER_DBTYPE$$", context.Provider.GetDbTypeString(c!))
                            .Replace("$NAME", c!.ColumnName.ToCamelCase())));

                    classesString.AppendLine(
                        Constants.EDM_VIEW_INTERNAL_CLASS_TEMPLATE
                            .Replace("$CLASSNAME", entity.SplitOnItem.Type)
                            .Replace("$PROPERTIES", classProperties));

                    tuplePropertiesString.AppendLine(string.Join(
                        Environment.NewLine,
                        entity.Columns.Select(c => Constants.EDM_PROPERTY_INDENTED_TEMPLATE
                            .Replace("$KEY", c.Index == 0 ? "Key, " : string.Empty)
                            .Replace("$COLUMN_NAME", c.ColumnName)
                            .Replace("$$PROVIDER_DBTYPE$$", context.Provider.GetDbTypeString(c!))
                            .Replace("$TYPE", sqlTransformer.GetCSharpType(c.DataType, c?.MaxLength ?? "250", c?.DataTypePrecision, c?.DataTypeScale, c?.IsNullable ?? false))
                            .Replace("$NAME", c!.ColumnName.ToCamelCase()))));
                }

                // tuple properties/class
                tupleString.AppendLine(
                    Constants.EDM_VIEW_TUPLE_CLASS_TEMPLATE
                        .Replace("$CLASSNAME", entities.First().Value.SplitOnItem.Type)
                        .Replace("$PROPERTIES", tuplePropertiesString.ToString()));
            }

            await File.WriteAllTextAsync(
                Path.Combine(outputDirectory, "Views", $"{context.ModelsPrefix}{viewDescriptor.Value.ViewName.ToCamelCase()}{context.ModelsSufix}.cs"),
                Constants.EDM_VIEW_CLASS_TEMPLATE
                    .Replace("$$PROVIDER_CLIENT_NAMESPACE$$", context.Provider.GetProviderClientUsingString())
                    .Replace("$NAMESPACE", context.Namespace ?? "-")
                    .Replace("$CLASSNAME", $"{context.ModelsPrefix}{viewDescriptor.Value.ViewName.ToCamelCase()}{context.ModelsSufix}")
                    .Replace("$TABLE", viewDescriptor.Value.ViewName)
                    .Replace("$SCHEMA", viewDescriptor.Value.SchemaName)
                    .Replace("$PROPERTIES", propsString.ToString())
                    .Replace("$REFERENCES", refsString.ToString())
                    .Replace("$CLASSES", classesString.ToString())
                    .Replace("$TUPLE", tupleString.ToString())
                    .Replace("$PROVIDER", context.Provider.ToString())
            );
        }
    }
}
