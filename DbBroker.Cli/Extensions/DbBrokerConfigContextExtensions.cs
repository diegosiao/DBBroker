using System;
using System.Data.Common;
using System.Data.SqlClient;
using DbBroker.Cli.Model;
using DbBroker.Cli.Services.Interfaces;
using DbBroker.Cli.Services.Providers.SqlServer;

namespace DbBroker.Cli.Extensions;

public static class DbBrokerConfigContextExtensions
{
    public static DbConnection GetDbConnection(this DbBrokerConfigContext context)
    {
        switch (context.Provider)
        {
            case SupportedDatabaseProviders.SqlServer:
                return new SqlConnection(context.ConnectionString);
            default:
                throw new ArgumentException("Provider not supported.");
        }
    }

    public static IMetadataProvider GetMetadataProvider(this DbBrokerConfigContext context)
    {
        switch (context.Provider)
        {
            case SupportedDatabaseProviders.SqlServer:
                return new SqlServerMetadataProvider();
            default:
                throw new ArgumentException("Provider not supported.");
        }
    }
}
