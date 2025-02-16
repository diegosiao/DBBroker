using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlSelectAvgCommand<TDataModel, TResult> : SqlSelectAggregateCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    public SqlSelectAvgCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, column, connection, transaction, Constants.SqlSelectAvgTemplate)
    {
    }
}