using System;
using System.Data.Common;
using DbBroker.Common;
using DBBroker.Model;

namespace DBBroker.Extensions;

public static class ResolversExtensions
{
    public static DbConnection GetConnection<TDataModel>(this DataModelBase<TDataModel> dataModelBase)
    {
        return null;
    }

    public static DbParameter GetDbParameter(this SupportedDatabaseProviders provider, string name, object value)
    {
        return provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("System.Data.SqlClient.SqlParameter, System.Data.SqlClient"), name, value) as DbParameter,
            _ => throw new ArgumentException("Not supported database provider"),
        };
    }
}
