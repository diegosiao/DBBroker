using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlUpdateCommand<TDataModel> : SqlCommand<TDataModel, int> where TDataModel : DataModel<TDataModel>
{
    internal SqlUpdateCommand(
        TDataModel dataModel,
        IEnumerable<DataModelMapProperty> columns,
        IEnumerable<DbParameter> parameters,
        DbConnection connection,
        DbTransaction transaction) :
        base(dataModel, columns, parameters, connection, transaction, Constants.SqlUpdateTemplate)
    {
        RequireFilter = true;
    }
}
