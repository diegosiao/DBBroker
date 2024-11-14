using System.Data.SqlClient;
using EShop.DataModels;
using DbBroker;
using DbBroker.Model;

Console.WriteLine("DBBroker showtime!");
Console.WriteLine("==================");
Console.WriteLine(
@"
- Make sure you have installed DBBroker.Cli and executed 'dbbroker sync' to generate the Data Models.
- Make sure you have the database containers running.

    > docker-compose -f dbbroker-showcase-docker-compose.yaml up
    > dotnet install -g DbBroker.Cli
    > dbbroker init
    > dbbroker sync

For more information go to https://nuget.org/DBBroker.Cli

All good? Showtime now? (y/n)

");

if (!Console.ReadLine()?.ToLower().Equals("y") ?? true)
{
    return;
}

// (Too much SQL) Dapper ------->  [ DBBroker ⭐ ]  <------- EF (Too much management)

CustomersDataModel customer = new()
{
    Id = Guid.NewGuid(),
    Name = "John Three Sixteen",
    CreatedAt = DateTime.Now,
    CreatedBy = "Diego",
};

OrdersDataModel order = new()
{
    Id = Guid.NewGuid(),
    StatusId = 1,
    CustomerId = customer.Id,
    CreatedAt = DateTime.Now,
    CreatedBy = "Diego",
};

var connection = new SqlConnection("Server=127.0.0.1;Database=DbBroker;User Id=sa;Password=sa123;");
connection.Open();

// using a transaction
var transaction = connection.BeginTransaction();
connection.Insert(customer, transaction);
connection.Insert(order, transaction);

transaction.Commit();

Console.WriteLine($"New customer and order inserted. ");

order.StatusId = 2;
order.ModifiedAt = DateTime.Now.AddMinutes(5);
order.ModifiedBy = "Diego";

// Updating multiple records
var rowsAffected = connection.Update(order)
    .AddFilter(x => x.StatusId, SqlEquals.To(1))
    .Execute();

Console.WriteLine($"{rowsAffected} row(s) updated. ");

// Deleting a record by Id
rowsAffected = connection.Delete<OrderStatusDataModel>()
    .AddFilter(x => x.Id, SqlEquals.To(6))
    .Execute();

Console.WriteLine($"{rowsAffected} row(s) deleted. ");

// Retrieving complex data
var customers = connection
    .Select<CustomersDataModel>(
        include: [
            (x => x.Id),
            (x => x.Name),
            (x => x.CustomersNotesCustomerIdRefs)
        ],
        depth: 2)
    .AddFilter(x => x.Id, SqlEquals.To("31AF4AC2-5BC6-4050-AA0C-543BBE7BB99D"))
    .Execute();

Console.WriteLine(string.Join(",", customers.Select(x => x.Name)));
Console.WriteLine(string.Join(",", customers.Select(x => x.CreatedBy)));

Console.WriteLine($"{customers.Count()} customer(s) retrieved.");
