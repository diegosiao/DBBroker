using EShop.DataModels.Oracle;
using DbBroker;
using System.Diagnostics;
using DbBroker.Extensions;
using DbBroker.Showcase.Cli.Seeders;

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

Console.WriteLine(@"
Which database you would like to use?

[1] SQL Server
[2] Oracle

Input the the option number: ");

Console.WriteLine(Guid.NewGuid().ToString());
Console.WriteLine(Guid.NewGuid().ToString());

OracleSeeder.Seed();

// (Too much SQL) Dapper ------->  [ DBBroker ⭐ ]  <------- EF (Too much management)

using var connection = typeof(CustomersDataModel)
    .GetConnection("user id=dbbroker;password=DBBroker_1;data source=//localhost:1529/xe;");

// Retrieving data
var orders = connection
    .Select<OrdersDataModel>(
        load: [
            // Root properties (depth 0)
            (x => x.CreatedAt),
            (x => x.CreatedBy),

            // Just 'Name' for Customer (depth 1)
            (x => x.CustomerIdRef!.Name), 
            
            // Just 'Country' and 'State' for Address (depth 2)
            (x => x.CustomerIdRef!.AddressIdRef!.Country), 
            (x => x.CustomerIdRef!.AddressIdRef!.State),
            
            // Explicitly loading collections
            (x => x.OrdersProductsOrderIdRefs),
            (x => x.OrdersNotesOrderIdRefs)
        ],
        depth: 2,
        skip: 1,
        take: 2)
    .OrderBy(x => x.CreatedBy)
    .OrderBy(x => x.CustomerIdRef!.Name, ascending: false)
    //.AddFilter(x => x.CustomerId, SqlEquals.To(OracleSeeder.customerId_1))
    .Execute();

Console.WriteLine($"Orders retrieved: {orders.Count()}");
Console.WriteLine($"Customers: {string.Join(',', orders.Select(x => x.CustomerIdRef!.Name))}");
Console.WriteLine($"Press any key to finish...");
// Console.Read();
