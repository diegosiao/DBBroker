using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlSelectSumCommand<TDataModel, TResult> : SqlSelectAggregateCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    public SqlSelectSumCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, column, connection, transaction, Constants.SqlSelectSumTemplate)
    {
    }
}