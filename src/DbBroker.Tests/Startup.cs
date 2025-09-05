using Microsoft.Data.SqlClient;
using DbBroker.Tests.Config;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DbBroker.Unit.Tests;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<ConnectionStrings>(c => {
            c.SqlServer = Configuration.GetConnectionString("SqlServer");
            c.Oracle = Configuration.GetConnectionString("Oracle");
        });
        services.AddSingleton(sp =>
        {
            return new OracleConnection(sp.GetService<IOptions<ConnectionStrings>>()?.Value.Oracle);
        });

        services.AddSingleton(sp =>
        {
            return new SqlConnection(sp.GetService<IOptions<ConnectionStrings>>()?.Value.SqlServer);
        });
    }
}
