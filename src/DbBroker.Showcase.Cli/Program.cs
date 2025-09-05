using DbBroker;
using System.Diagnostics;
using DbBroker.Extensions;
using DbBroker.Tests.DataModels.Oracle;

Console.Write(
@"
==================
DBBroker Showtime!
==================

This project is a showcase of DBBroker capabilities.

Start databases:

    > docker-compose up --build

Once the databases are ready, install DBBroker CLI and generate the Data Models:

    > dotnet tool install --global DbBroker.Cli --prerelease
    > dbbroker sync

A running container does not mean the database is ready.
Check the databases containers logs or healthcheck status to make sure the databases are ready.

Why should I use DBBroker?
.NET ADO         -> SQL +++++  Code +++++
Dapper           -> SQL +++++  Code ++++
Dapper.Contrib   -> SQL ++     Code +++
BBroker          -> SQL +      Code +

All good? Showtime now? 
(y/n): ");

if (!Debugger.IsAttached && (!Console.ReadLine()?.ToLower().Equals("y") ?? true))
{
    return;
}

Console.WriteLine(@"
Which database you would like to use?

[1] SQL Server
[2] Oracle

: ");

Console.WriteLine(Guid.NewGuid().ToString());
Console.WriteLine(Guid.NewGuid().ToString());

// Creating connection from a Data Model type
using var connection = typeof(CustomersDataModel)
    .GetConnection("user id=dbbroker;password=DBBroker_1;data source=//localhost:1529/xe;");

// Retrieving data
// var orders = connection
//     .Select<OrdersDataModel>(
//         load: [
//             // Root properties (depth 0)
//             (x => x.CreatedAt),
//             (x => x.CreatedBy),

//             // Just 'Name' for Customer (depth 1)
//             (x => x.CustomerIdRef!.Name), 
            
//             // Just 'Country' and 'State' for Address (depth 2)
//             (x => x.CustomerIdRef!.AddressIdRef!.Country),
//             (x => x.CustomerIdRef!.AddressIdRef!.State),
            
//             // Explicitly loading collections
//             (x => x.OrdersProductsOrderIdRefs),
//             (x => x.OrdersNotesOrderIdRefs)
//         ],
//         depth: 2,
//         skip: 1,
//         take: 2)
//     .OrderBy(x => x.CreatedBy)
//     .OrderBy(x => x.CustomerIdRef!.Name, ascending: false)
//     //.AddFilter(x => x.CustomerId, SqlEquals.To(OracleSeeder.customerId_1))
//     .Execute();

// var orderSummary = connection
//     .Select<UvwOrderSummaryDataModel>()
//     .OrderBy(x => x.StatusId)
//     .OrderBy(x => x.UvwOrderSummaryCustomerRef!.Name)
//     .Execute();

// var o = connection
//     .Insert(new UvwOrderSummaryDataModelTuple());

try
{
    var count = connection
            .Count<OrdersDataModel>()
            .AddFilter(x => x.Id, SqlEquals.To(new Guid("5a0642ca-1710-48b6-9381-93f4072fee9d").ToByteArray()))
            .Execute();

    var sum = connection
            .Sum<OrdersDataModel, decimal>(x => x.StatusId)
            .Execute();

    var avg = connection
            .Avg<OrdersDataModel, decimal>(x => x.StatusId)
            .Execute();

    var min = connection
            .Min<OrdersDataModel, decimal>(x => x.StatusId)
            .Execute();

    var max = connection
        .Max<OrdersDataModel, decimal>(x => x.StatusId)
        .Execute();

}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

// Console.WriteLine($"Customers: {string.Join(",", orders.Select(x => x.CustomerIdRef!.Name))}");
Console.WriteLine($"Press any key to finish...");
Console.Read();
