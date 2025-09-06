using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DbBroker.Extensions;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL AGGREGATE command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
/// <typeparam name="TResult"></typeparam>
public abstract class SqlSelectAggregateCommand<TDataModel, TResult> : SqlCommand<TDataModel, TResult> where TDataModel : DataModel<TDataModel>
{
    /// <summary>
    /// Constructor for SQL AGGREGATE command
    /// </summary>
    /// <param name="dataModel"></param>
    /// <param name="column"></param>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    /// <param name="sqlTemplate"></param>
    public SqlSelectAggregateCommand(
        TDataModel dataModel,
        DataModelMapProperty column,
        DbConnection connection,
        DbTransaction transaction,
        string sqlTemplate) : base(dataModel, [column], [], connection, transaction, sqlTemplate)
    {
    }

    /// <summary>
    /// Renders the SQL command
    /// </summary>
    /// <returns></returns>
    internal protected override string RenderSqlCommand()
    {
        return SqlTemplate
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", Columns.First().ColumnName)
            .Replace("$$FILTERS$$", Filters.RenderWhereClause()); // TODO: Expose a configuration flag to avoid not filtered UPDATE or DELETE by default
    }

    /// <summary>
    /// Executes the AGGREGATE command and returns the result
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public override TResult Execute(int commandTimeout = 0)
    {
        try
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            List<DbParameter> filterParameters = [];
            foreach (var filter in Filters)
            {
                filterParameters.AddRange(filter.Parameters);
            }

            var command = Connection.CreateCommand();
            command.CommandText = RenderSqlCommand();
            command.Parameters.AddRange(Parameters.ToArray());
            command.Parameters.AddRange(filterParameters.ToArray());
            command.Transaction = Transaction;
            command.CommandTimeout = commandTimeout;

            Debug.WriteLine(command.CommandText);

            var result = command.ExecuteScalar();

            if (result is null)
            {
                return default;
            }

            return (TResult)command.ExecuteScalar();
        }
        catch
        {
            Debug.WriteLine(RenderSqlCommand());
            throw;
        }
    }
}
