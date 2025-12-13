using Microsoft.Data.SqlClient;
using DbBroker.Tests.DataModels.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace DbBroker.Tests.Providers.SqlServer;

public class CustomerTests(ServiceProviderFixture fixture) : IClassFixture<ServiceProviderFixture>
{
    private readonly SqlConnection _sqlConnection = fixture.ServiceProvider.GetService<SqlConnection>()!;

    [Fact]
    public void CanCreateCustomer()
    {
        CustomersDataModel customer = new()
        {
            Id = Guid.NewGuid(),
            Name = "John Three Sixteen",
            Birthday = DateTime.Now.AddYears(-30),
            Orderstotal = 20,
            Createdat = DateTime.UtcNow,
            Createdby = Environment.UserName,
        };

        var rowsAffected = _sqlConnection.Insert(customer).Execute();
        Assert.Equal(1, rowsAffected);
    }
}
