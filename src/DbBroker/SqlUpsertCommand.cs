using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Common;
using DbBroker.Model;

namespace DbBroker;

// TODO it may not make sense to inherit from SqlCommand
public class SqlUpsertCommand<TDataModel> : SqlCommand<TDataModel, int> where TDataModel : DataModel<TDataModel>
{
    private const string SqlUpsertOracleTemplate =
@"MERGE INTO $$TABLEFULLNAME$$ t
$$USING$$
ON (t.$$KEYCOLUMN$$ = s.$$KEYCOLUMNPARAM$$)
WHEN MATCHED THEN
    $$UPDATE$$
WHEN NOT MATCHED THEN
    $$INSERT$$";

    private const string SqlUpsertOracleUsingTemplate = "USING (SELECT $$COLUMNS$$ FROM dual) s";

    private const string SqlUpsertSqlServerTemplate =
@"MERGE INTO $$TABLEFULLNAME$$ AS t
$$USING$$
ON t.$$KEYCOLUMN$$ = s.$$KEYCOLUMNPARAM$$
WHEN MATCHED THEN
    $$UPDATE$$
WHEN NOT MATCHED THEN
    $$INSERT$$";

    private const string SqlUpsertSqlServerUsingTemplate = "USING (VALUES (@id, @value)) AS s (id, value)";

    internal SqlUpsertCommand(
        TDataModel dataModel,
        IEnumerable<DataModelMapProperty> columns,
        IEnumerable<DbParameter> parameters,
        DbConnection connection,
        DbTransaction transaction) :
        base(dataModel, columns, parameters, connection, transaction, string.Empty)
    {
        RequireFilter = true;

        switch (dataModel.DataModelMap.Provider)
        {
            case SupportedDatabaseProviders.SqlServer:
                SqlTemplate = SqlUpsertSqlServerTemplate;
                break;
            case SupportedDatabaseProviders.Oracle:
                SqlTemplate = SqlUpsertOracleTemplate;
                break;
        }
    }

    protected override string RenderSqlCommand()
    {
        var keyPropertyMap = DataModel.DataModelMap.MappedProperties.FirstOrDefault(x => x.Value.IsKey);

        return SqlTemplate
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$USING$$", SqlUpsertOracleUsingTemplate.Replace("$$COLUMNS$$", string.Join(",", Parameters.Select(x => $"{x.ParameterName} AS {x.ParameterName[1..]}"))))
            .Replace("$$KEYCOLUMN$$", keyPropertyMap.Value.ColumnName)
            .Replace("$$KEYCOLUMNPARAM$$", keyPropertyMap.Value.ColumnName)
            .Replace("$$UPDATE$$", string.Join(",", $"UPDATE SET {string.Join(",", Columns.Where(c => !c.IsKey).Select(x => $"t.{x.ColumnName} = s.{x.ColumnName}"))}"))
            .Replace("$$INSERT$$", $"INSERT ({string.Join(",", Columns.Select(x => x.ColumnName))}) VALUES ({string.Join(",", Columns.Select(x => $"s.{x.ColumnName}"))})");
    }
}
