using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DbBroker.Common;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker.Extensions;

public static class ResolversExtensions
{
    /// <summary>
    /// Creates and returns a <see cref="DbConnection" /> new instance according to the Data Model type provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model as argument type</typeparam>
    /// <param name="dataModelBase">An instance of any Data Model from the target context resolved by its Namespace</param>
    /// <param name="connectionString">Specify if you want to ignore 'dbbroker.config.json' connection string value</param>
    /// <returns>A new instance of <see cref="DbConnection" />.</returns>
    public static DbConnection GetConnection(this Type dataModelType, string connectionString = null)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            // get from dbbroker.config.json or application settings
        }

        if (Activator.CreateInstance(dataModelType) is not IDataModel dataModelBase)
        {
            throw new ArgumentException($"The type '{dataModelType.FullName}' is not a valid Data Model");
        }

        return dataModelBase.DataModelMap.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("System.Data.SqlClient.SqlConnection, System.Data.SqlClient"), connectionString) as DbConnection,
            SupportedDatabaseProviders.Oracle => Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleConnection, Oracle.ManagedDataAccess"), connectionString) as DbConnection,
            _ => throw new ArgumentException($"Not supported database provider: {dataModelBase?.DataModelMap?.Provider}"),
        };
    }

    /// <summary>
    /// Creates and returns a <see cref="DbConnection" /> new instance according to the Data Model provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model as argument type</typeparam>
    /// <param name="dataModelBase">An instance of any Data Model from the target context resolved by its Namespace</param>
    /// <param name="connectionString">Specify if you want to ignore 'dbbroker.config.json' connection string value</param>
    /// <returns>A new instance of <see cref="DbConnection" />.</returns>
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

    public static DbType GetDbType(this SupportedDatabaseProviders provider, PropertyInfo propertyInfo)
    {
        // TODO WIP https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/data-type-mappings-in-ado-net
        return provider switch
        {
            SupportedDatabaseProviders.Oracle => GetOracleDbType(propertyInfo),
            SupportedDatabaseProviders.SqlServer => GetSqlServerDbType(propertyInfo),
            _ => throw new ArgumentException($"Not supported database provider: {provider}"),
        };
    }

    private static DbType GetSqlServerDbType(PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType == typeof(decimal))
        {
            return DbType.Decimal;
        }

        if (new Type[]{ typeof(int), typeof(byte) }.Contains(propertyInfo.PropertyType))
        {
            return DbType.Int32;
        }

        return DbType.String;
    }

    private static DbType GetOracleDbType(PropertyInfo propertyInfo)
    {
        if (new Type[] { typeof(int), typeof(byte) }.Contains(propertyInfo.PropertyType))
        {
            return DbType.Int32;
        }

        if (propertyInfo.PropertyType == typeof(decimal))
        {
            return DbType.Decimal;
        }

        return DbType.String;
    }
}
