using System;
using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL SELECT BY KEY command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class SqlSelectByKeyCommand<TDataModel> : SqlCommand<TDataModel, TDataModel> where TDataModel : DataModel<TDataModel>
{
    /// <summary>
    /// Constructor for SQL SELECT BY KEY command
    /// </summary>
    /// <param name="dataModel"></param>
    /// <param name="connection"></param>
    /// <param name="columns"></param>
    /// <param name="primaryKey"></param>
    /// <param name="depth"></param>
    /// <param name="transaction"></param>
    public SqlSelectByKeyCommand(
        TDataModel dataModel,
        DbConnection connection,
        IEnumerable<DataModelMapProperty> columns,
        DbParameter primaryKey,
        int depth = 0,
        DbTransaction transaction = null) : base(dataModel, columns, [primaryKey], connection, transaction, Constants.SqlSelectCountTemplate)
    {
    }

    /// <summary>
    /// Executes the SELECT BY KEY command and returns the result
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override TDataModel Execute(int commandTimeout = 0)
    {
        throw new NotImplementedException();
    }
}
