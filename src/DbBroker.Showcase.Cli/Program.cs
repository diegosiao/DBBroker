using EShop.DataModels.Oracle;
using DbBroker;
using DbBroker.Model;
using Oracle.ManagedDataAccess.Client;
using System.Diagnostics;

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

if (!Debugger.IsAttached && (!Console.ReadLine()?.ToLower().Equals("y") ?? true))
{
    return;
}



// (Too much SQL) Dapper ------->  [ DBBroker ⭐ ]  <------- EF (Too much management)

CustomersDataModel customer = new()
{
    Id = Guid.NewGuid().ToByteArray(),
    Name = "John Three Sixteen",
    CreatedAt = DateTime.Now,
    CreatedBy = "Diego",
};

OrdersDataModel order = new()
{
    Id = Guid.NewGuid().ToByteArray(),
    StatusId = 1,
    CustomerId = customer.Id,
    CreatedAt = DateTime.Now,
    CreatedBy = "Diego",
};

var connection = new OracleConnection("user id=dbbroker;password=password;data source=//localhost:1529/xe;");
connection.Open();

// using a transaction
var transaction = connection.BeginTransaction();
try
{
    connection.Insert(customer, transaction);
    connection.Insert(order, transaction);
    transaction.Commit();
}
catch (Exception ex)
{
    transaction.Rollback();
    Console.WriteLine(ex.Message);
}

Console.WriteLine($"New customer and order inserted. ");

order = new()
{
    StatusId = 2,
    ModifiedAt = DateTime.Now.AddMinutes(5),
    ModifiedBy = "Diego"
};

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
    .AddFilter(x => x.Id, SqlEquals.To(customer.Id))
    .Execute();

Console.WriteLine(string.Join(",", customers.Select(x => x.Name)));
Console.WriteLine(string.Join(",", customers.Select(x => x.CreatedBy)));

Console.WriteLine($"{customers.Count()} customer(s) retrieved.");
