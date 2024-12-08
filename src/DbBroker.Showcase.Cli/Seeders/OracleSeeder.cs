using System.Data.Common;
using DbBroker.Extensions;
using DbBroker.Model;
using EShop.DataModels.Oracle;

namespace DbBroker.Showcase.Cli.Seeders;

public static class OracleSeeder
{
    public readonly static byte[] customerId_1 = new Guid("80202d51-cf03-7840-9f81-7a65e97830ec").ToByteArray();

    public readonly static byte[] customerId_2 = new Guid("47748fba-db36-4502-b2df-06ef2c7502fd").ToByteArray();

    public readonly static byte[] customerId_3 = new Guid("fd1d24b4-6daa-4e95-bb33-9fa57fdc66d9").ToByteArray();

    private static List<ProductsDataModel> products = [
        new() {
            Id = Guid.NewGuid().ToByteArray(),
            ProductName = "Banana"
        },
        new() {
            Id = Guid.NewGuid().ToByteArray(),
            ProductName = "Orange"
        },
        new() {
            Id = Guid.NewGuid().ToByteArray(),
            ProductName = "Apple"
        },
        new() {
            Id = Guid.NewGuid().ToByteArray(),
            ProductName = "Grape"
        },
    ];

    public static void Seed()
    {
        using var connection = typeof(CustomersDataModel).GetConnection("user id=dbbroker;password=DBBroker_1;data source=//localhost:1529/xe;");

        // check if seeded already
        if (connection.Select<CustomersDataModel>().AddFilter(x => x.Id, SqlEquals.To(customerId_1)).Execute().Any())
        {
            return;
        }

        var ordersStatus = new List<string> { "New", "Pending", "Finished", "Canceled" };
        ordersStatus.ForEach(x =>
        {
            var order = new OrderStatusDataModel
            {
                Status = x
            };
            connection.Insert(order);
        });

        try
        {
            SeedCustomerAndOrders_1(connection);
            SeedCustomerAndOrders_2(connection);
            SeedCustomerAndOrders_3(connection);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void SeedCustomerAndOrders_1(DbConnection connection)
    {
        AddressDataModel address = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Street = "Av. Pres. Café Filho, 400",
            State = "RN",
            PostalCode = "59010000",
            City = "Natal",
            Country = "Brazil"
        };

        CustomersDataModel customer = new()
        {
            Id = customerId_1,
            AddressId = address.Id,
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

        using var transaction = connection.BeginTransaction();
        try
        {
            connection.Insert(address, transaction);
            connection.Insert(customer, transaction);
            connection.Insert(order, transaction);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
        }

        products.ForEach(product =>
        {
            connection.Insert(product);
            connection.Insert(new OrdersProductsDataModel
            {
                Id = Guid.NewGuid().ToByteArray(),
                OrderId = order.Id,
                ProductId = product.Id,
            });
        });

        var orderNotes = new List<OrdersNotesDataModel>{
            new(){
                OrderId = order.Id,
                NoteContent = "Note content 1",
            },
            new(){
                OrderId = order.Id,
                NoteContent = "Note content 2",
            },
            new(){
                OrderId = order.Id,
                NoteContent = "Note content 3",
            },
            new(){
                OrderId = order.Id,
                NoteContent = "Note content 4",
            },
            new(){
                OrderId = order.Id,
                NoteContent = "Note content 5",
            },
        };

        orderNotes.ForEach(note =>
        {
            connection.Insert(note);
        });
    }

    private static void SeedCustomerAndOrders_2(DbConnection connection)
    {
        AddressDataModel address = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Street = "Av. Prudente de Morais, 5121",
            State = "RN",
            PostalCode = "59064-625",
            City = "Natal",
            Country = "Brazil"
        };

        CustomersDataModel customer = new()
        {
            Id = customerId_2,
            AddressId = address.Id,
            Name = "Luke Two",
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

        using var transaction = connection.BeginTransaction();
        try
        {
            connection.Insert(address, transaction);
            connection.Insert(customer, transaction);
            connection.Insert(order, transaction);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
        }

        products.Skip(3).ToList().ForEach(product =>
        {
            connection.Insert(new OrdersProductsDataModel
            {
                Id = Guid.NewGuid().ToByteArray(),
                OrderId = order.Id,
                ProductId = product.Id,
            });
        });

        var orderNotes = new List<OrdersNotesDataModel>{
            new(){
                OrderId = order.Id,
                NoteContent = "[Customer 2] Note content 1",
            },
            new(){
                OrderId = order.Id,
                NoteContent = "[Customer 2] Note content 2",
            }
        };

        orderNotes.ForEach(note =>
        {
            connection.Insert(note);
        });
    }

    private static void SeedCustomerAndOrders_3(DbConnection connection)
    {
        AddressDataModel address = new()
        {
            Id = Guid.NewGuid().ToByteArray(),
            Street = "Av. Pres. Café Filho, 1",
            State = "RN",
            PostalCode = "59010000",
            City = "Natal",
            Country = "Brazil"
        };

        CustomersDataModel customer = new()
        {
            Id = customerId_3,
            AddressId = address.Id,
            Name = "Matthew Seven",
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

        using var transaction = connection.BeginTransaction();
        try
        {
            connection.Insert(address, transaction);
            connection.Insert(customer, transaction);
            connection.Insert(order, transaction);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
        }

        products.TakeLast(1).ToList().ForEach(product =>
        {
            connection.Insert(new OrdersProductsDataModel
            {
                Id = Guid.NewGuid().ToByteArray(),
                OrderId = order.Id,
                ProductId = product.Id,
            });
        });

        var orderNotes = new List<OrdersNotesDataModel>{
            new(){
                OrderId = order.Id,
                NoteContent = "[Customer 3] Note content 1",
            }
        };

        orderNotes.ForEach(note =>
        {
            connection.Insert(note);
        });
    }
}
