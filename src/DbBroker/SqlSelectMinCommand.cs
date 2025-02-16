using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlSelectMinCommand<TDataModel, TResult> : SqlSelectAggregateCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    public SqlSelectMinCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, column, connection, transaction, Constants.SqlSelectMinTemplate)
    {
    }
}