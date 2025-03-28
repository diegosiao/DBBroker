using System.Data.Common;
using System.Data.SqlClient;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Cli.Services.Providers;
using DbBroker.Cli.Services.Providers.Oracle;
using DbBroker.Cli.Services.Providers.SqlServer;
using DbBroker.Common;
using DbBroker.Common.Model;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Cli.Extensions;

public static class DbBrokerConfigContextExtensions
{
    private static Dictionary<SupportedDatabaseProviders, IProviderDefaultConfiguration> _defaultProviderConfigs = new()
    {
        { SupportedDatabaseProviders.SqlServer, new SqlServerDefaultConfiguration() },
        { SupportedDatabaseProviders.Oracle, new OracleDefaultConfiguration() },
    };

    public static DbConnection GetDbConnection(this DbBrokerConfigContext context)
    {
        return context.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => new SqlConnection(context.ConnectionString),
            SupportedDatabaseProviders.Oracle => new OracleConnection(context.ConnectionString),
            _ => throw new ArgumentException($"Provider not supported: {context.Provider}"),
        };
    }

    // TODO Do an extensive revision on access modifiers: CLI and lib
    internal static IMetadataProvider GetMetadataProvider(this DbBrokerConfigContext context)
    {
        return context.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => new SqlServerMetadataProvider(),
            SupportedDatabaseProviders.Oracle => new OracleMetadaProvider(),
            _ => throw new ArgumentException($"Provider not supported: {context.Provider}."),
        };
    }

    public static IProviderDefaultConfiguration GetDefaultProviderConfig(this DbBrokerConfigContext context)
    {
        return _defaultProviderConfigs[context.Provider!.Value];
    }

    public static ISqlTransformer GetSqlTransformer(this DbBrokerConfigContext context)
    {
        return context.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => new SqlServerSqlTransformer(),
            SupportedDatabaseProviders.Oracle => new OracleSqlTransformer(),
            _ => throw new ArgumentException($"Provider not supported: {context.Provider}"),
        };
    }
}
