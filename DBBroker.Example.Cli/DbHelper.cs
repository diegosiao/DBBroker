using System;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;

namespace DBBroker.Example.Cli;

public class DbHelper
{

public class Program
{
    public static void Main()
    {
        /*
        var connection = new SqlConnection("");

        // Get the generic method info
        var methodInfo = typeof(SqlMapper)
            .GetMethod("Query", [ 
                typeof(SqlConnection), 
                typeof(string), 
                typeof(object), 
                typeof(SqlTransaction), 
                typeof(bool), 
                typeof(string), 
                typeof(int?), 
                typeof(CommandType?) ]);

        // Make the method generic
        var genericMethod = methodInfo.MakeGenericMethod(typeof(Order), typeof(Customer), typeof(Product), typeof(Order));

        // Parameters for the method
        var parameters = new object[]
        {
            connection,
            sql,
            null, // parameters
            null, // transaction
            true, // buffered
            "CustomerId,ProductId", // splitOn
            null, // commandTimeout
            null  // commandType
        };

        // Invoke the method
        var result = genericMethod.Invoke(null, parameters);

        // Convert the result to a List of Order
        var orders = result as IEnumerable<Order>;
        foreach (var order in orders)
        {
            Console.WriteLine(order);
        }
        */
    }
}

// Define your classes
public class Order
{
    public int OrderId { get; set; }
    public string Status { get; set; }
    public Customer Customer { get; set; }
    public Product Product { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
}

public class Product
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
}

}
