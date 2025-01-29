using System.Data.Common;
using Dapper;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers;

public abstract class MetadataProvider(string sqlSelectColumns, string sqlSelectKeys) : IMetadataProvider
{
    private readonly string _sqlSelectColumns = sqlSelectColumns;
    private readonly string _sqlSelectKeys = sqlSelectKeys;

    public virtual async Task<Dictionary<string, TableDescriptorModel>> GetTableDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context)
    {
        Dictionary<string, TableDescriptorModel> tableDescriptors = [];

        var columns = await connection.QueryAsync<ColumnDescriptorModel>(_sqlSelectColumns);
        var keys = await connection.QueryAsync<KeyDescriptorModel>(_sqlSelectKeys);

        foreach (var tableColumns in columns.GroupBy(x => $"{x.SchemaName}.{x.TableName}"))
        {
            var tableDescriptor = new TableDescriptorModel
            {
                SchemaName = tableColumns.FirstOrDefault()?.SchemaName ?? string.Empty,
                TableName = tableColumns.FirstOrDefault()?.TableName ?? string.Empty,
                Columns = [.. tableColumns]
            };
            tableDescriptor.Keys = keys
                .Where(x => x.SchemaName.Equals(tableDescriptor.SchemaName) && x.TableName.Equals(tableDescriptor.TableName))
                .ToArray();

            tableDescriptors.Add(tableDescriptor.TableFullName, tableDescriptor);
        }

        return tableDescriptors;
    }
}
