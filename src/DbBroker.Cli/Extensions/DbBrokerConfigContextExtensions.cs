using System.Data.Common;
using System.Data.SqlClient;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Cli.Services.Providers;
using DbBroker.Cli.Services.Providers.Oracle;
using DbBroker.Cli.Services.Providers.SqlServer;
using DbBroker.Common;
using DbBroker.Common.Model;

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
            _ => throw new ArgumentException("Provider not supported."),
        };
    }

    public static IMetadataProvider GetMetadataProvider(this DbBrokerConfigContext context)
    {
        return context.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => new SqlServerMetadataProvider(),
            _ => throw new ArgumentException("Provider not supported."),
        };
    }

    public static IProviderDefaultConfiguration GetDefaultProviderConfig(this DbBrokerConfigContext context)
    {
        return _defaultProviderConfigs[context.Provider];
    }

    public static ISqlTransformer GetSqlTransformer(this DbBrokerConfigContext context)
    {
        return context.Provider switch
        {
            SupportedDatabaseProviders.SqlServer => new SqlServerSqlTransformer(),
            SupportedDatabaseProviders.Oracle => new OracleSqlTransformer(),
            _ => throw new ArgumentException("Provider not supported."),
        };
    }
}
