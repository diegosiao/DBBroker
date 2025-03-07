using System.Data.Common;
using Dapper;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers;

public abstract class MetadataProvider(string sqlSelectTablesColumns, string sqlSelectViewsColumns, string sqlSelectKeys) : IMetadataProvider
{
    public virtual async Task<Dictionary<string, TableDescriptorModel>> GetTableDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        Dictionary<string, TableDescriptorModel> tableDescriptors = [];

        var columns = await connection.QueryAsync<ColumnDescriptorModel>(sqlSelectTablesColumns);
        var keys = await connection.QueryAsync<KeyDescriptorModel>(sqlSelectKeys);

        foreach (var tableColumns in columns.GroupBy(x => $"{x.SchemaName}.{x.TableName}"))
        {
            var tableDescriptor = new TableDescriptorModel
            {
                SchemaName = tableColumns.FirstOrDefault()?.SchemaName ?? string.Empty,
                TableName = tableColumns.FirstOrDefault()?.TableName ?? string.Empty,
                Columns = [.. tableColumns]
            };
            tableDescriptor.Keys = [.. keys.Where(x => x.SchemaName.Equals(tableDescriptor.SchemaName) && x.TableName.Equals(tableDescriptor.TableName))];

            tableDescriptors.Add(tableDescriptor.TableFullName, tableDescriptor);
        }
        return tableDescriptors;
    }

    public async Task<Dictionary<string, ViewDescriptorModel>> GetViewsDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        Dictionary<string, ViewDescriptorModel> viewsDescriptors = [];

        var columns = await connection.QueryAsync<ColumnDescriptorModel>(sqlSelectViewsColumns);
        
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
        return viewsDescriptors;
    }
}
