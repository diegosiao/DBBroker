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

Start databases:

    > docker-compose up --build

Once the databases are ready, generate the Data Models:

    > dotnet install -g DbBroker.Cli
    > dbbroker sync

A running container does not mean the database is ready.
Check the databases containers logs to make sure the databases are ready.

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
// var transaction = connection.BeginTransaction();
// try
// {
//     connection.Insert(customer, transaction);
//     connection.Insert(order, transaction);
//     transaction.Commit();
// }
// catch (Exception ex)
// {
//     transaction.Rollback();
//     Console.WriteLine(ex.Message);
// }

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

// var products = connection
//     .Select<ProductsDataModel>()
//     // .AddFilter(x => x.Id, SqlEquals.To(new Guid("8b4a2060-b387-ba4b-bdfe-99b830198083").ToByteArray()))
//     .Execute();

// var product = connection
//     .Select<ProductsDataModel>()
//     .AddFilter(x => x.Id, SqlEquals.To(products.ElementAt(2).Id))
//     .Execute();

// try
// {
//     var product1 = new OrdersProductsDataModel
//     {
//         Id = Guid.NewGuid().ToByteArray(),
//         OrderId = Guid.Parse("80202d51-cf03-7840-9f81-7a65e97830ec").ToByteArray(true),
//         ProductId = product.First().Id
//     };

//     connection.Insert(product1);
// }
// catch (Exception ex)
// {
//     Console.WriteLine(ex.Message);
// }

// Retrieving data
var orders = connection
    .Select<OrdersDataModel>(
        include: [
            (x => x.CreatedAt),
            (x => x.CreatedBy),
            (x => x.CustomerIdRef.Name), // Just 'Name' for Customer (depth 1)
            (x => x.CustomerIdRef.AddressIdRef.Country), // Just 'country' and 'state' for Address (depth 2)
            (x => x.CustomerIdRef.AddressIdRef.State),
            (x => x.OrdersProductsOrderIdRefs) // Explicitly loading a collection
        ],
        depth: 2)
    //.AddFilter(x => x.Id, SqlEquals.To(customer.Id))
    .Execute();

Console.WriteLine($"Orders retrieved: {orders.Count()}");
Console.WriteLine($"Orders retrieved?");

// var customers = connection
//     .Select<CustomersDataModel>(
//         include: [
//             (x => x.Name),
//             (x => x.CustomersNotesCustomerIdRefs)
//         ],
//         depth: 2)
//     // .AddFilter(x => x.Id, SqlEquals.To(customer.Id))
//     .Execute();

// Console.WriteLine(string.Join(",", customers.Select(x => x.Name)));
// Console.WriteLine(string.Join(",", customers.Select(x => x.CreatedBy)));

// Console.WriteLine($"{customers.Count()} customer(s) retrieved.");
