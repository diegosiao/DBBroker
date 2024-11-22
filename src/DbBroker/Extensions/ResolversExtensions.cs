using System;
using System.Data.Common;
using DbBroker.Common;
using DbBroker.Model;

namespace DbBroker.Extensions;

public static class ResolversExtensions
{
    public static DbConnection GetConnection<TDataModel>(this DataModel<TDataModel> dataModelBase)
    {
        return null;
    }

    public static DbParameter GetDbParameter(this SupportedDatabaseProviders provider, string name, object value)
    {
        return provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("System.Data.SqlClient.SqlParameter, System.Data.SqlClient"), $"@{name}", value) as DbParameter,
            SupportedDatabaseProviders.Oracle => Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleParameter, Oracle.ManagedDataAccess"), $":{name}", value) as DbParameter,
            _ => throw new ArgumentException("Not supported database provider"),
        };
    }
}
