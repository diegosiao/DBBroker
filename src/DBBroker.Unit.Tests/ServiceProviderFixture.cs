using System;
using Microsoft.Extensions.DependencyInjection;

namespace DBBroker.Unit.Tests;

public class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var startup = new Startup();
        var serviceCollection = new ServiceCollection();
        startup.ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        if (ServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
