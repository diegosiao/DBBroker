using System.Text;
using DbBroker.Cli.Extensions;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers.SqlServer;

public class CSharpClassGenerator : ICSharpClassGenerator
{
    public async Task<int> GenerateAsync(DbBrokerConfigContext context)
    {
        "Retrieving database metadata...".Log();
        
        using var connection = context.GetDbConnection();

        var sqlTransformer = context.GetSqlTransformer();

        var tableDescriptors = await context
            .GetMetadataProvider()
            .GetTableDescriptorsAsync(connection, context);

        var providerDefaultConfig = context.GetDefaultProviderConfig();

        foreach (var tableDescriptor in tableDescriptors)
        {
            var outputDirectory = (context.Namespace?.Split('.')?.Length > 1 ? string.Join('/', context.Namespace.Split('.').Skip(1)) : context.Namespace) ?? string.Empty;
            outputDirectory = Path.Combine(Directory.GetCurrentDirectory(), outputDirectory!);

            Directory.CreateDirectory(context.OutputDirectory ?? outputDirectory);
            
            var propsString = new StringBuilder();
            foreach (var item in tableDescriptor.Value.Columns)
            {
                var isPrimaryKey = tableDescriptor.Value.Keys.Any(x => x.KeyFullName.Equals(item.ColumnFullName) && x.ConstraintType.Equals("PrimaryKey"));

                propsString.AppendLine(
                    Constants.EDM_PROPERTY_TEMPLATE
                        .Replace("$TYPE", sqlTransformer.GetCSharpType(item.DataType, item?.MaxLength ?? "50", item?.IsNullable ?? false))
                        .Replace("$KEY", isPrimaryKey ? "Key, " : string.Empty)
                        .Replace("$COLUMN_NAME", item?.ColumnName)
                        .Replace("$NAME", item?.ColumnName.ToCamelCase()));
            }
            


            File.WriteAllText(
                Path.Combine(outputDirectory, $"{tableDescriptor.Value.TableName}DataModel.cs"),
                Constants.EDM_CLASS_TEMPLATE
                    .Replace("$NAMESPACE", context.Namespace ?? "-")
                    .Replace("$CLASSNAME", $"{tableDescriptor.Value.TableName}DataModel")
                    .Replace("$TABLE", tableDescriptor.Value.TableName)
                    .Replace("$SCHEMA", tableDescriptor.Value.SchemaName)
                    .Replace("$PROPERTIES", propsString.ToString())
                    .Replace("$PROVIDER", context.Provider.ToString())
                    .Replace("$ISQLINSERTTEMPLATETYPEFULLNAME", 
                        context.Tables.FirstOrDefault(x => x.Name.Equals(tableDescriptor.Value.TableName))?.SqlInsertTemplateType 
                        ?? context.DefaultSqlInsertTemplateTypeFullName
                        ?? providerDefaultConfig.ISqlInsertTemplateTypeFullName));
        }

        return 0;
    }
}
