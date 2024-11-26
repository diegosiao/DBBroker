using DbBroker;
using EShop.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DBBroker.Unit.Tests.Providers.Oracle;

public class CustomerTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    [Fact]
    public void CanCreateCustomer()
    {
        CustomersDataModel customer = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Name = "John Three Sixteen",
            Birthday = DateTime.Now.AddYears(-30),
            OrdersTotal = 20,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Environment.UserName,
        };

        var row = _oracleConnection.Insert(customer);
        Assert.True(row);
    }
}
