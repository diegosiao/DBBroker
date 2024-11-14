using System.Data.SqlClient;
using System.Linq.Expressions;
using EShop.DataModels;
using DbBroker;
using DbBroker.Model;
using Dapper;

Console.WriteLine("DBBroker showtime!");
Console.WriteLine("==================");

Console.WriteLine("- Make sure you executed 'dbbroker sync' to generate the Data Models. ");

// CustomersDataModel customer = new()
// {
//     Id = Guid.NewGuid(),
//     Name = "John Three Sixteen",
//     CreatedAt = DateTime.Now,
//     CreatedBy = "Diego",
// };

// OrdersDataModel order = new()
// {
//     Id = Guid.NewGuid(),
//     StatusId = 1,
//     CustomerId = customer.Id,
//     CreatedAt = DateTime.Now,
//     CreatedBy = "Diego",
// };

var connection = new SqlConnection("Server=127.0.0.1;Database=DbBroker;User Id=sa;Password=sa123;");
connection.Open();

// var transaction = connection.BeginTransaction();
// connection.Insert(customer, transaction);
// connection.Insert(order, transaction);

// transaction.Commit();

// Console.WriteLine($"New customer and order inserted. ");

// order.StatusId = 2;
// order.ModifiedAt = DateTime.Now.AddMinutes(5);
// order.ModifiedBy = "Diego";

// var rowsAffected = connection.Update(order)
//     .AddFilter(x => x.StatusId, SqlEquals.To(1))
//     .AddFilter(x => x.CustomerId, SqlEquals.To(customer.Id))
//     .Execute();

// Console.WriteLine($"{rowsAffected} row(s) updated. ");

// rowsAffected = connection.Delete<OrderStatusDataModel>()
//     .AddFilter(x => x.Id, SqlEquals.To(6))
//     .Execute();

// Console.WriteLine($"{rowsAffected} row(s) deleted. ");

// Type[] types = [
//     typeof(CustomersDataModel),
//     typeof(CustomersNotesDataModel), 
//     // typeof(OrdersDataModel),
//     // typeof(PromotionsEnrollmentsDataModel),
//     // typeof()
//     ];

// CustomersDataModel model = null;
// var lookup = new Dictionary<Guid, CustomersDataModel>();
// var result = connection.Query<CustomersDataModel, CustomersNotesDataModel, CustomersDataModel>(
//     @"select CustId, Name, Id, NoteContent
//     from Customers c
//     join CustomersNotes n on n.CustomerId = c.CustId
//     where c.CustId = '31AF4AC2-5BC6-4050-AA0C-543BBE7BB99D'; ",
//     //types: types,
//     map: (cust, note) =>
//     {
//         if (!lookup.TryGetValue(cust.CustId, out var currentCustomer)) 
//         { 
//             currentCustomer = cust; 
//             lookup.Add(currentCustomer.CustId, currentCustomer); 
//         }
//         if (note != null) 
//         { 
//             currentCustomer.CustomersNotesCustomerIdRefs.Append(note); 
//         }

//         Console.WriteLine(currentCustomer.GetHashCode() + " - what?");
//         return currentCustomer;
//     },
//     splitOn: "CustId,Id")
//     .ToArray();

// Console.WriteLine($"Result: {result.Length} records");

// CustomersDataModel model = null;
// var lookup = new Dictionary<Guid, CustomersDataModel>();

// var result = connection.Query<CustomersDataModel, CustomersNotesDataModel, CustomersDataModel>(
//     @"SELECT c.CustId, c.Name, n.Id, n.NoteContent
//       FROM Customers c
//       LEFT JOIN CustomersNotes n ON n.CustomerId = c.CustId
//       WHERE c.CustId = '31AF4AC2-5BC6-4050-AA0C-543BBE7BB99D';",
//     map: (cust, note) =>
//     {
//         if (!lookup.TryGetValue(cust.CustId, out var currentCustomer))
//         {
//             currentCustomer = cust;
//             currentCustomer.CustomersNotesCustomerIdRefs = new List<CustomersNotesDataModel>();
//             lookup.Add(currentCustomer.CustId, currentCustomer);
//         }
//         if (note != null)
//         {
//             currentCustomer.CustomersNotesCustomerIdRefs.Add(note);
//         }

//         Console.WriteLine(currentCustomer.GetHashCode() + " - what?");
//         return currentCustomer;
//     },
//     splitOn: "Id")
//     .Distinct()
//     .ToArray();

// // Output the results
// foreach (var customer in result)
// {
//     Console.WriteLine($"Customer: {customer.Name}");
//     foreach (var note in customer.CustomersNotesCustomerIdRefs)
//     {
//         Console.WriteLine($"  Note: {note.NoteContent}");
//     }
// }
// return;

var customers = connection
    .Select<CustomersDataModel>(
        include: [
            (x => x.CustId),
            (x => x.Name),
            (x => x.CustomersNotesCustomerIdRefs)
        ],
        depth: 2)
    .AddFilter(x => x.CustId, SqlEquals.To("31AF4AC2-5BC6-4050-AA0C-543BBE7BB99D"))
    .Execute();

Console.WriteLine(string.Join(",", customers.Select(x => x.Name)));
Console.WriteLine(string.Join(",", customers.Select(x => x.CreatedBy)));

Console.WriteLine($"{customers.Count()} record(s) retrieved.");

var orders = connection
    .Select<OrdersDataModel>(
        include: [],
        depth: 2)
    .AddFilter(x => x.Id, SqlEquals.To("31AF4AC2-5BC6-4050-AA0C-543BBE7BB99D"))
    .Execute();

Console.WriteLine(orders.Count() + " orders");


// var hashSet = new HashSet<CustomersDataModel>(ReferenceEqualityComparer.Instance)
// {
//     customersNotes[0].CustomerIdRef,
//     costumersNotes[1].CustomerIdRef,
//     costumersNotes[2].CustomerIdRef
// }; 

//Console.WriteLine($"HashSet contains {hashSet.Count} items.");

// var costumers = connection
//     .Select<CustomersDataModel>(
//         columns: [
//             (x => x.Name),
//         ],
//         orderByAsc: [
//             (x => x.Name),
//             (x => x.Birthday)
//         ],
//         depth: 1)
//     .AddFilter(x => x.CreatedBy, SqlEquals.To("Diego Morais"))
//     .Execute();

// Console.WriteLine("COSTUMERS RETRIEVED:");
// Console.WriteLine("===================");
// foreach (var costumer in costumers)
// {
//     Console.WriteLine($"Name: {costumer.Name}");
// }

// dotnet dbbroker init
// dotnet dbbroker sync

// [Customers] synced
// [Cars] synced
// Finished. Project up-to-date with database schema.

public class OrderEdm // Entity Data Model: Mapeamento direto OU DataModel
{
    public int StatusId { get; set; }
}

public class OrdersPendingLem : OrderEdm // Entity Relation Model OU LogicModel
{
    public string Name { get; set; }

    public CustomerEdm Customer { get; set; }

    public ListEdm<ProductEdm> Products { get; set; }

    public ListEdm<OrderNoteEdm> OrderNotes { get; set; }
}

public class CustomerEdm
{

}

public class ProductEdm
{
    public string Name { get; set; }
}

public class OrderNoteEdm
{

}

public record OrderProductsEdm
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public OrderEdm Order { get; set; } = new();

    public ProductEdm Product { get; set; } = new();
}

/// <summary>
/// Soluciona o problema do mapeamento de listas
/// </summary>
/// <typeparam name="T">Base Entity Data Model</typeparam>
public class ListEdm<T> : List<T> where T : class // DataModel<T>
{
    public T? DataModel { get; set; }
}

public static class Playground
{
    public static void Play()
    {
        var orders = new OrderProductsEdm();
        orders.ProductId = Guid.NewGuid();
        orders.OrderId = Guid.NewGuid();

        orders.Order = new OrderEdm();
        orders.Order.StatusId = 2;

        // (Too much SQL) DAPPER -------->  [ DbBroker ]  <-------- EF (Zero SQL)

        /*
        dotnet install -g DbBroker.Cli  (dotnet tool)
        dotnet add package DbBroker.Core (library)

        ----------------------

        var result = await _dbbroker
            .Select<OrderEdm>(depth: 3)
                .AddFilter(order => order.Cpf, Sql.Equals, "01358267480")
                .AddFilter(order => order.StatusId, Sql.Equals, 1)
                .AddFilter(order => order.CreatedAt, Sql.Between, new [ DateTime.Now.Date, DateTime.Now.Date.AddDays(1) ])
                .AddFilter(order => order.Products.DataModel.Name, Sql.Like, "%tooth paste%", toLower: true)

                .Slice(skip: 0, take: 50)

            .QueryAsync();

        var result = await _dbbroker
            .Insert<Order>(order, transaction); // transaction, no implicit commit

        var result = await _dbbroker
            .Delete<Order>(order) // [Key] based, no transaction, implicit commit
                .AddFilter(order => order.StatusId == 2);

        var result = await _dbbroker
            .Update<Order>(order) // [Key] based, ignore pristine properties (not changed)
                .AddFilter(order => order.StatusId == 3);


        */

        Console.WriteLine(PropertyPathHelper.GetPropertyPath<OrdersPendingLem, string>(x => x.Products.DataModel.Name));
        Console.WriteLine(PropertyPathHelper.GetNestedPropertyPath<OrdersPendingLem, string>(x => x.Products.DataModel.Name));
    }
}


///
public static class PropertyPathHelper
{
    public static string GetPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
    {
        var member = propertyLambda.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException("Expression inválida", nameof(propertyLambda));

        return member.Member.Name;
    }

    public static string GetNestedPropertyPath<T, TProperty>(Expression<Func<T, TProperty>> propertyLambda)
    {
        var member = propertyLambda.Body as MemberExpression;
        if (member == null)
            throw new ArgumentException("Expression inválida", nameof(propertyLambda));

        var path = member.Member.Name;
        while (member.Expression is MemberExpression innerMember)
        {
            path = innerMember.Member.Name + "." + path;
            member = innerMember;
        }

        return path;
    }
}
