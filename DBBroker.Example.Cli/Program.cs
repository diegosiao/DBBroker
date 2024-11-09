using System.Collections;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Xml.XPath;
using eShop.DataModels;
using DbBroker;
using Dapper;

Console.WriteLine("Hello, World!");

Playground.Play();

var customer = new Customers();
customer.Name = "";

var connection = new SqlConnection("");
connection.DbInsert(customer, null);



// dotnet nuget add source C:\Users\USER\git\DBBroker\DBBroker.Core\bin\Debug -n DBBrokerNugetSource

// dotnet add package DBBroker --source DBBrokerNugetSource

// dotnet dbbroker init
// dotnet dbbroker sync

// [Customers] synced
// [Cars] synced
// Finished. Project up-to-date with database schema.

/*
var connection = new SqlConnection("your_connection_string"); 
var sql = "SELECT o.OrderId, o.Status, c.CustomerId, c.Name, p.ProductId, p.ProductName FROM Orders o " + "INNER JOIN Customers c ON o.CustomerId = c.CustomerId " + "INNER JOIN Products p ON p.OrderId = o.OrderId"; // Get the generic method info var methodInfo = typeof(SqlMapper) .GetMethod("Query", new[] { typeof(SqlConnection), typeof(string), typeof(object), typeof(SqlTransaction), typeof(bool), typeof(string), typeof(int?), typeof(CommandType?) }); // Make the method generic var genericMethod = methodInfo.MakeGenericMethod(typeof(Order), typeof(Customer), typeof(Product), typeof(Order)); // Parameters for the method var parameters = new object[] { connection, sql, null, // parameters null, // transaction true, // buffered "CustomerId,ProductId", // splitOn null, // commandTimeout null // commandType }; // Invoke the method var result = genericMethod.Invoke(null, parameters); // Convert the result to a List of Order var orders = result as IEnumerable<Order>; foreach (var order in orders) { Console.WriteLine(order); }

var result = await _dbbroker
    .Select<OrdersInProductionElm>(depth: 3)
    .AddFilter(order => order.CreatedAd, Sql.Between,  new [ DateTime.Now.Date, DateTime.Now.Date.AddDays(1) ])
    .AddFilter(order => order.StatusId, Sql.Equals, 1)
    .AddFilter(order => order.Products.DataModel.Name, Sql.Like, "%tooth paste%", toLower: true)
    .QueryAsync();

*/

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
public class ListEdm<T> : List<T> where T : class
{
    public T DataModel { get; set; }
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


//////
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
