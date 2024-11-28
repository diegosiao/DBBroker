using System.Data;
using EShop.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Unit.Tests.Providers.Oracle;

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

        var customerInserted = _oracleConnection.Insert(customer);
        Assert.True(customerInserted);
    }

    [Fact]
    public void CanUseTransactions()
    {
        if (_oracleConnection.State != ConnectionState.Open)
        {
            _oracleConnection.Open();
        }

        var transaction = _oracleConnection.BeginTransaction();
        CustomersDataModel customer = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Name = "John Three Sixteen",
            Birthday = DateTime.Now.AddYears(-30),
            OrdersTotal = 20,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Environment.UserName,
        };

        OrdersDataModel order = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            CustomerId = customer.Id,
            StatusId = 1,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Environment.UserName,
        };

        var customerInserted = _oracleConnection.Insert(customer, transaction);
        var orderInserted = _oracleConnection.Insert(order, transaction);

        transaction.Commit();

        Assert.True(customerInserted && orderInserted);
    }
}
