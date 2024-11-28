using EShop.DataModels.Oracle;
using DbBroker;
using DbBroker.Model;
using System.Diagnostics;
using DbBroker.Extensions;

Console.Write(
@"
==================
DBBroker Showtime!
==================

- Make sure you have installed DBBroker.Cli and executed 'dbbroker sync' to generate the Data Models.
- Make sure you have the database containers running and ready.

    > docker-compose up --build
    > dotnet install -g DbBroker.Cli
    > dbbroker sync

For more information go to https://nuget.org/packages/DBBroker.Cli or https://nuget.org/packages/DBBroker

All good? Showtime now? 
(y/n): ");

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
    CreatedBy = Environment.UserName,
};

OrdersDataModel order = new()
{
    Id = Guid.NewGuid().ToByteArray(),
    StatusId = 1,
    CustomerId = customer.Id,
    CreatedAt = DateTime.Now,
    CreatedBy = Environment.UserName,
};

var connection = customer.GetConnection("user id=dbbroker;password=DBBroker_1;data source=//localhost:1529/xe;");
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

Console.WriteLine(@"

New customer and order inserted. ");

order = new()
{
    StatusId = 2,
    ModifiedAt = DateTime.Now,
    ModifiedBy = Environment.UserName
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

// Retrieving data
var customers = connection
    .Select<CustomersDataModel>(
        include: [
            (x => x.ALL),
            (x => x.CustomersNotesCustomerIdRefs)
        ],
        depth: 2)
    .AddFilter(x => x.Id, SqlEquals.To(customer.Id))
    .Execute();

Console.WriteLine(string.Join(",", customers.Select(x => x.Name)));
Console.WriteLine(string.Join(",", customers.Select(x => x.CreatedBy)));

Console.WriteLine($"{customers.Count()} customer(s) retrieved.");
