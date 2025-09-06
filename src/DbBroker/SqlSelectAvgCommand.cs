using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL AVG command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SqlSelectAvgCommand<TDataModel, TResult> : SqlSelectAggregateCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    /// <summary>
    /// Constructor for SQL AVG command
    /// </summary>
    /// <param name="dataModel"></param>
    /// <param name="column"></param>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    public SqlSelectAvgCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, column, connection, transaction, Constants.SqlSelectAvgTemplate)
    {
    }
}