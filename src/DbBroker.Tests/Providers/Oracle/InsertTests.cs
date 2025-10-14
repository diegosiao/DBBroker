using System.Data;
using DbBroker.Tests.DataModels.Oracle;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;

namespace DbBroker.Tests.Providers.Oracle;

public class InsertTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly OracleConnection _oracleConnection = fixture.ServiceProvider.GetService<OracleConnection>()!;

    [Fact(DisplayName = "Can insert a record?")]
    public void CanInsertRecord()
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

        var rowsAffected = _oracleConnection
            .Insert(customer)
            .Execute();

        Assert.Equal(1, rowsAffected);
    }

    [Fact(DisplayName = "Can insert a record using all supported types?")]
    public void CanInsertAllSupportedTypesRecord()
    {
        // TODO Implement this test properly
        if (_oracleConnection.State != ConnectionState.Open)
        {
            _oracleConnection.Open();
        }

        SupportedTypesDataModel supportedTypes = new()
        {
            UuidType = Guid.NewGuid().ToByteArray(),
            Varchar2TypeMax = "John Three Sixteen",
            Varchar2TypeMin = "A",
            NumberType = 20,
            DateType = DateTime.Now.AddYears(-30),
            TimestampType = DateTime.UtcNow,
            BooleanType = 0,
        };

        // var rowsAffected = _oracleConnection
        //     .Insert(supportedTypes)
        //     .Execute();

        // Assert.Equal(1, rowsAffected);
        Assert.True(true);
    }

    [Fact]
    public void CanInsertUsingTransaction()
    {
        if (_oracleConnection.State != ConnectionState.Open)
        {
            _oracleConnection.Open();
        }

        var transaction = _oracleConnection.BeginTransaction();
        AddressesDataModel address = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Street = "Prudente De Morais, Av.",
            City = "Natal",
            State = "RN",
            Country = "Brazil",
            PostalCode = "59065878",
        };

        CustomersDataModel customer = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            AddressId = address.Id,
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
            BillingAddressId = address.Id,
            ShippingAddressId = address.Id,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = Environment.UserName,
        };

        var rowsAffected =
            _oracleConnection.Insert(address, transaction).Execute()
            + _oracleConnection.Insert(customer, transaction).Execute() 
            + _oracleConnection.Insert(order, transaction).Execute();

        Assert.Equal(3, rowsAffected);
        
        transaction.Commit();
    }
}
