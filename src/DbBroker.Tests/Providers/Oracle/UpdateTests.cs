using System.Data.Common;
using DbBroker.Model;
using DbBroker.Tests.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Tests.Providers.Oracle;

public class UpdateTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    private void UpdateRecord(DbTransaction? transaction)
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

        _oracleConnection.Insert(customer, transaction);

        var customerInserted = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute()
            .FirstOrDefault();

        Assert.True(customerInserted is not null, "Unable to retrieve the customer inserted.");

        if (customerInserted is null)
        {
            return;
        }

        customerInserted.Name = "John Fourteen Six";

        var rowsAffected = _oracleConnection
            .Update(customerInserted, transaction)
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute();

        transaction?.Commit();

        Assert.Equal(1, rowsAffected);

        var customerUpdated = _oracleConnection
            .Select<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(customerId.ToByteArray()))
            .Execute()
            .FirstOrDefault();

        Assert.Equal(customerInserted.Name, customerUpdated?.Name);
    }

    [Fact]
    public void CanUpdate()
    {
        UpdateRecord(transaction: null);
    }

    [Fact]
    public void CanUpdateUsingTransaction()
    {
        if (_oracleConnection.State != System.Data.ConnectionState.Open)
        {
            _oracleConnection.Open();
        }

        UpdateRecord(transaction: _oracleConnection.BeginTransaction());
    }
}
