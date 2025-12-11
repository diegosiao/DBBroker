using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// Select SQL MAX command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SqlSelectMaxCommand<TDataModel, TResult> : SqlSelectAggregateCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    /// <summary>
    /// Constructor for SQL MAX command
    /// </summary>
    /// <param name="dataModel"></param>
    /// <param name="column"></param>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    public SqlSelectMaxCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, column, connection, transaction, Constants.SqlSelectMaxTemplate)
    {
    }
}
