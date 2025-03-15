using System.Data.Common;
using System.Diagnostics;
using Dapper;
using DbBroker.Cli.Extensions;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers;

internal abstract class MetadataProvider(string sqlSelectTablesColumns, string sqlSelectViewsColumns, string sqlSelectKeys) : IMetadataProvider
{
    public abstract string GetTableFilterExpression(DbBrokerConfigContext context, bool checkAddReferencesConfig = false);

    public abstract string GetViewsFilterExpression(DbBrokerConfigContext context);

    public virtual async Task<Dictionary<string, TableDescriptorModel>> GetTableDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        if (!context.Tables.Any())
        {
            return [];
        }

        var stopwatch = Stopwatch.StartNew();
        Dictionary<string, TableDescriptorModel> tableDescriptors = [];

        var tablesFilterExpression = GetTableFilterExpression(context);

        var sqlColumns = sqlSelectTablesColumns.Replace("$$TABLESFILTER$$", tablesFilterExpression);
        sqlColumns.Debug(context.Namespace);
        var columns = await connection.QueryAsync<ColumnDescriptorModel>(sqlColumns);

        var sqlKeys = sqlSelectKeys
                .Replace("$$PRIMARYKEYS$$", tablesFilterExpression)
                .Replace("$$FOREIGNKEYS$$", GetTableFilterExpression(context, true));
        sqlKeys.Debug(context.Namespace);
        var keys = await connection.QueryAsync<KeyDescriptorModel>(sqlKeys);

        foreach (var tableColumns in columns.GroupBy(x => $"{x.SchemaName}.{x.TableName}"))
        {
            var tableDescriptor = new TableDescriptorModel
            {
                SchemaName = tableColumns.FirstOrDefault()?.SchemaName ?? string.Empty,
                TableName = tableColumns.FirstOrDefault()?.TableName ?? string.Empty,
                Columns = [.. tableColumns]
            };

            var contextTable = context.Tables.First(t => t.Name.ToLower().Equals(tableDescriptor.TableName.ToLower()));
            tableDescriptor.Keys = [..
                keys.Where(x =>
                    x.SchemaName.Equals(tableDescriptor.SchemaName)
                    && x.TableName.Equals(tableDescriptor.TableName)
                    && (x.ConstraintType != "PrimaryKey" || contextTable.PrimaryKeyColumn is null || x.ColumnName.ToLower().Equals(contextTable.PrimaryKeyColumn.ToLower())))];

            tableDescriptors.Add(tableDescriptor.TableFullName, tableDescriptor);
        }

        $"{stopwatch.Elapsed.TotalSeconds:N2} seconds to retrieve tables metadata".Log(context.Namespace);
        return tableDescriptors;
    }

    public async Task<Dictionary<string, ViewDescriptorModel>> GetViewsDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        if (!context.Views.Any())
        {
            return [];
        }

        var stopwatch = Stopwatch.StartNew();
        Dictionary<string, ViewDescriptorModel> viewsDescriptors = [];

        var columns = await connection.QueryAsync<ColumnDescriptorModel>(
            sqlSelectViewsColumns.Replace("$$VIEWSFILTER$$", GetViewsFilterExpression(context)));

        foreach (var viewColumns in columns.GroupBy(x => $"{x.SchemaName}.{x.ViewName}"))
        {
            var viewDescriptor = new ViewDescriptorModel
            {
                SchemaName = viewColumns.FirstOrDefault()?.SchemaName ?? string.Empty,
                ViewName = viewColumns.FirstOrDefault()?.ViewName ?? string.Empty,
                Columns = [.. viewColumns]
            };

            viewsDescriptors.Add(viewDescriptor.ViewFullName, viewDescriptor);
        }

        $"{stopwatch.Elapsed.TotalSeconds:N2} seconds to retrieve views metadata".Log(context.Namespace);
        return viewsDescriptors;
    }
}
