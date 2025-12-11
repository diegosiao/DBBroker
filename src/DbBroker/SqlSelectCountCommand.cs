using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL SELECT COUNT command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class SqlSelectCountCommand<TDataModel> : SqlCommand<TDataModel, long> where TDataModel : DataModel<TDataModel>
{
    internal SqlSelectCountCommand(
        TDataModel dataModel,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, [], [], connection, transaction, Constants.SqlSelectCountTemplate)
    {
    }
    
    /// <summary>
    /// Executes the SELECT COUNT command and returns the result
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    public override long Execute(int commandTimeout = 0)
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

            return long.Parse(command.ExecuteScalar().ToString());
        }
        catch
        {
            Debug.WriteLine(RenderSqlCommand());
            throw;
        }
    }
}
