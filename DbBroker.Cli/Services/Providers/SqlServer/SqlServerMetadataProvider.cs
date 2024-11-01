using System.Data.Common;
using Dapper;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerMetadataProvider : IMetadataProvider
{
    public async Task<Dictionary<string, TableDescriptorModel>> GetTableDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        Dictionary<string, TableDescriptorModel> tableDescriptors = [];
        
        var columns = await connection.QueryAsync<ColumnDescriptorModel>(SqlServerConstants.SELECT_COLUMNS);
        var keys = await connection.QueryAsync<KeyDescriptorModel>(SqlServerConstants.SELECT_KEYS);

        foreach (var tableColumns in columns.GroupBy(x => $"{x.SchemaName}.{x.TableName}"))
        {
            var tableDescriptor = new TableDescriptorModel
            {
                SchemaName = tableColumns.FirstOrDefault()?.SchemaName ?? string.Empty,
                TableName = tableColumns.FirstOrDefault()?.TableName ?? string.Empty,
            };

            tableDescriptor.Columns = [.. tableColumns];
            tableDescriptor.Keys = keys
                .Where(x => x.SchemaName.Equals(tableDescriptor.SchemaName) && x.TableName.Equals(tableDescriptor.TableName))
                .ToArray();

            tableDescriptors.Add(tableDescriptor.TableFullName, tableDescriptor);
        }

        return tableDescriptors;
    }
}
