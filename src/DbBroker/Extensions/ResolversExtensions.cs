using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using DbBroker.Common;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker.Extensions;

/// <summary>
/// Provides extension methods for resolvers
/// </summary>
public static class ResolversExtensions
{
    /// <summary>
    /// Creates and returns a <see cref="DbConnection" /> new instance according to the Data Model type provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <param name="dataModelType">The Data Model as argument type</param>
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
            // TODO get from dbbroker.config.json or application settings
        }

        return dataModelBase.DataModelMap.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => Activator.CreateInstance(Type.GetType("Microsoft.Data.SqlClient.SqlConnection, Microsoft.Data.SqlClient"), connectionString) as DbConnection,
            SupportedDatabaseProviders.Oracle => Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleConnection, Oracle.ManagedDataAccess"), connectionString) as DbConnection,
            _ => throw new ArgumentException($"Not supported database provider: {dataModelBase.DataModelMap.Provider}"),
        };
    }

    /// <summary>
    /// Creates and returns a <see cref="DbParameter" /> new instance according to the database provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DbParameter GetDbParameter(this SupportedDatabaseProviders provider, string name, object value)
    {
        switch (provider)
        {
            case SupportedDatabaseProviders.SqlServer:
                return Activator.CreateInstance(Type.GetType("Microsoft.Data.SqlClient.SqlParameter, Microsoft.Data.SqlClient"), $"@{name}", value) as DbParameter;

            case SupportedDatabaseProviders.Oracle:
                return Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleParameter, Oracle.ManagedDataAccess"), $":{name}", value) as DbParameter;

            default:
                throw new ArgumentException($"Not supported database provider: {provider}");
        }
    }

    /// <summary>
    /// Creates and returns a <see cref="DbParameter" /> new instance according to the database provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// <para>Uses the <see cref="DataModelMapProperty" /> to get the parameter name and type.</para>
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="dataModel"></param>
    /// <param name="dataModelMapProperty"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DbParameter GetDbParameter(this SupportedDatabaseProviders provider, object dataModel, DataModelMapProperty dataModelMapProperty)
    {
        switch (provider)
        {
            case SupportedDatabaseProviders.SqlServer:
                return Activator.CreateInstance(Type.GetType("Microsoft.Data.SqlClient.SqlParameter, Microsoft.Data.SqlClient"), $"@{dataModelMapProperty.ColumnName}", dataModelMapProperty.PropertyInfo.GetValue(dataModel)) as DbParameter;

            case SupportedDatabaseProviders.Oracle:
                var parameter = Activator.CreateInstance(Type.GetType("Oracle.ManagedDataAccess.Client.OracleParameter, Oracle.ManagedDataAccess")) as DbParameter;
                parameter.ParameterName = $":{dataModelMapProperty.ColumnName}";
                parameter.Value = dataModelMapProperty.PropertyInfo.GetValue(dataModel);
                //parameter.DbType = GetOracleDbType(dataModelMapProperty.PropertyInfo);
                parameter.GetType().GetProperty("OracleDbType").SetValue(parameter, dataModelMapProperty.ProviderDbType);
                return parameter;

            default:
                throw new ArgumentException($"Not supported database provider: {provider}");
        }
    }

    /// <summary>
    /// Gets the <see cref="DbType" /> for a given property according to the database provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="propertyInfo"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
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

        if (integerOracleTypes.Contains(propertyInfo.PropertyType))
        {
            return DbType.Int32;
        }

        if (dateTimeOracleTypes.Contains(propertyInfo.PropertyType))
        {
            return DbType.DateTime;
        }



        throw new ArgumentException($"Database type mapping not available: {propertyInfo.PropertyType.Name}");
    }

    /// <summary>
    /// Gets the client namespace import statement for a given database provider.
    /// <para>Check the <see cref="SupportedDatabaseProviders" /> for details on the required database client library reference.</para>
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static string GetClientNamespace(this SupportedDatabaseProviders provider)
    {
        return provider switch
        {
            SupportedDatabaseProviders.Oracle => "using Oracle.ManagedDataAccess.Client;",
            _ => throw new NotImplementedException($"Client namespace not implemented for {provider}"),
        };
    }

    private static readonly Type[] dateTimeOracleTypes = [typeof(DateTime), typeof(DateTime?)];

    private static readonly Type[] integerOracleTypes = [typeof(int), typeof(byte)];

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

        if (integerOracleTypes.Contains(propertyInfo.PropertyType))
        {
            return DbType.Int32;
        }

        if (dateTimeOracleTypes.Contains(propertyInfo.PropertyType))
        {
            return DbType.DateTime;
        }

        throw new ArgumentException($"Database type mapping not available: {propertyInfo.PropertyType.Name}");
    }
}
