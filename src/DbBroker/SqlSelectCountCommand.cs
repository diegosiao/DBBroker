using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

public class SqlSelectCountCommand<TDataModel> : SqlCommand<TDataModel, long> where TDataModel : DataModel<TDataModel>
{
    internal SqlSelectCountCommand(
        TDataModel dataModel,
        DbConnection connection,
        DbTransaction transaction) : base(dataModel, [], [], connection, transaction, Constants.SqlSelectCountTemplate)
    {
    }

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
