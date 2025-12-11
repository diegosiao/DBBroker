using System.Data.Common;
using DbBroker.Tests.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Tests.Providers.Oracle;

public class DeleteTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    private void DeleteRecord(DbTransaction? transaction)
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

        _oracleConnection
            .Insert(customer, transaction)
            .Execute();

        var rowsAffected = _oracleConnection
            .Delete<CustomersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(new Guid(customer.Id).ToByteArray()))
            .Execute();

        transaction?.Commit();

        Assert.Equal(1, rowsAffected);
    }

    [Fact]
    public void CanDeleteRecord()
    {
        DeleteRecord(transaction: null);
    }

    [Fact]
    public void CanDeleteUsingTransaction()
    {
        if (_oracleConnection.State != System.Data.ConnectionState.Open)
        {
            _oracleConnection.Open();
        }

        DeleteRecord(transaction: _oracleConnection.BeginTransaction());
    }
}
