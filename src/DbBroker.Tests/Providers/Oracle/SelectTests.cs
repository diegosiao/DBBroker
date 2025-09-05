using DbBroker.Tests.DataModels.Oracle;
using DbBroker.Tests;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Tests.Providers.Oracle;

public class SelectTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    [Fact]
    public void CanSelectRecord()
    {
        var customerId = Guid.NewGuid();

        CustomersDataModel customer = new()
        {
            Id = customerId.ToByteArray(),
            Name = "John Three Sixteen",
            Birthday = DateTime.Now.AddYears(-30),
            OrdersTotal = 20,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Environment.UserName,
        };

        var rowsAffected = _oracleConnection
            .Insert(customer)
            .Execute();

        Assert.Equal(1, rowsAffected);

        var result = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute();

        Assert.Single(result);
    }
}
