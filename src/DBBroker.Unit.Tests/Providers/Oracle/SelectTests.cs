using DbBroker.Model;
using EShop.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Unit.Tests.Providers.Oracle;

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

        var customerInserted = _oracleConnection.Insert(customer);
        Assert.True(customerInserted);

        var customerSelected = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute();

        Assert.True(customerSelected.Count() == 1);
    }
}
