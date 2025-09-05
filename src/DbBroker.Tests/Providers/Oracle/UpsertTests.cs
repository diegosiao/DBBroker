using DbBroker.Model;
using DbBroker.Tests.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Unit.Tests.Providers.Oracle;

public class UpsertTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    [Fact]
    public void CanUpsertRecord()
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

        _oracleConnection.Upsert(customer);

        var customerInserted = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute()
            .FirstOrDefault();
        
        Assert.True(customerInserted is not null);

        customer.Name = "John Fourteen Six";

        _oracleConnection.Upsert(customer);

        var customerUpdated = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute()
            .FirstOrDefault();

        Assert.Equal(customer.Name, customerUpdated?.Name);
    }
}
