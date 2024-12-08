using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlUpsertCommand<TDataModel> : SqlCommand<TDataModel, int> where TDataModel : DataModel<TDataModel>
{
    private const string SqlUpsertOracleTemplate =
@"MERGE INTO $$TABLEFULLNAME$$ t
$$USING$$ -- USING (SELECT :id AS id, :value AS value FROM dual) s
ON (t.$$KEYCOLUMN$$ = s.$$KEYCOLUMN$$)
WHEN MATCHED THEN
    $$UPDATE$$ -- UPDATE SET t.value = s.value
WHEN NOT MATCHED THEN
    $$INSERT$$ -- INSERT (id, value) VALUES (s.id, s.value);";

    private const string SqlUpsertSqlServerTemplate =
@"MERGE INTO $$TABLEFULLNAME$$ AS t
$$USING$$ --USING (VALUES (@id, @value)) AS s (id, value)
ON t.$$KEYCOLUMN$$ = s.$$KEYCOLUMN$$
WHEN MATCHED THEN
    $$UPDATE$$ -- UPDATE SET t.value = s.value
WHEN NOT MATCHED THEN
    $$INSERT$$ -- INSERT (id, value) VALUES (s.id, s.value);";

    internal SqlUpsertCommand(
        TDataModel dataModel,
        IEnumerable<DataModelMapProperty> columns,
        IEnumerable<DbParameter> parameters,
        DbConnection connection,
        DbTransaction transaction) :
        base(dataModel, columns, parameters, connection, transaction, Constants.SqlUpdateTemplate) { }
}
