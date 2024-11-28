using System;
using System.Data.Common;
using DbBroker.Common;
using DbBroker.Common.Model;
using DbBroker.Common.Model.Interfaces;
using DbBroker.Model;

namespace DbBroker.Extensions;

public static class ResolversExtensions
{
    private static readonly DbBrokerConfig _dbBrokerConfig;

    /// <summary>
    /// Creates and returns a <see cref="DbConnection" /> new instance according to the Data Model provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model as argument type</typeparam>
    /// <param name="dataModelBase">An instance of any Data Model from the target context resolved by its Namespace</param>
    /// <param name="connectionString">Specify if you want to ignore 'dbbroker.config.json' connection string value</param>
    /// <returns>A <see cref="DbConnection" /> new instance.</returns>
    public static DbConnection GetConnection<TDataModel>(this DataModel<TDataModel> dataModelBase, string connectionString = null)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            // get from dbbroker.config.json or application settings
        }

        return dataModelBase.DataModelMap.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("System.Data.SqlClient.SqlConnection, System.Data.SqlClient"), connectionString) as DbConnection,
            SupportedDatabaseProviders.Oracle => Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleConnection, Oracle.ManagedDataAccess"), connectionString) as DbConnection,
            _ => throw new ArgumentException($"Not supported database provider: {dataModelBase.DataModelMap.Provider}"),
        };
    }

    public static DbParameter GetDbParameter(this SupportedDatabaseProviders provider, string name, object value)
    {
        return provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("System.Data.SqlClient.SqlParameter, System.Data.SqlClient"), $"@{name}", value) as DbParameter,
            SupportedDatabaseProviders.Oracle => Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleParameter, Oracle.ManagedDataAccess"), $":{name}", value) as DbParameter,
            _ => throw new ArgumentException($"Not supported database provider: {provider}"),
        };
    }
}
