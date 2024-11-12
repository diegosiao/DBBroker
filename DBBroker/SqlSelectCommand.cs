using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbBroker.Extensions;
using DbBroker.Model;
using Dapper;
using System;
using DbBroker.Model.Interfaces;

namespace DbBroker;

public class SqlSelectCommand<TDataModel> : SqlCommand<TDataModel, IEnumerable<TDataModel>> where TDataModel : DataModel<TDataModel>
{
    private readonly int _maxDepth;

    private readonly List<SqlJoin> _joins;

    internal SqlSelectCommand(
        TDataModel dataModel,
        DbConnection connection,
        DbTransaction transaction,
        int depth) :
        base(dataModel, columns: [], parameters: [], connection, transaction, Constants.SqlSelectTemplate)
    {
        _maxDepth = depth;
        _joins = [];
    }

    private void LoadJoins(int depth, IDataModel dataModel, string parentAliasName)
    {
        if (depth >= _maxDepth)
        {
            return;
        }

        int joinIndex = 0;
        foreach (var reference in dataModel.DataModelMap.MappedReferences)
        {
            var join = new SqlJoin
            {
                Depth = depth,
                Index = ++joinIndex,
                SchemaName = reference.Value.SchemaName,
                TableName = reference.Value.TableName,
                ColumnName = reference.Value.ColumnName,
                ColumnAllowNulls = reference.Value.ColumnAllowNulls,
                TableAliasName = parentAliasName,
                RefTableName = reference.Value.RefTableName,
                RefColumnName = reference.Value.RefColumnName,
                RefPropertyInfo = reference.Value.PropertyInfo
            };

            _joins.Add(join);
            LoadJoins(depth + 1, (IDataModel)Activator.CreateInstance(reference.Value.PropertyInfo.PropertyType), join.TableAliasName);
        }
    }

    protected override string RenderSqlCommand()
    {
        if (_maxDepth > 0)
        {
            LoadJoins(0, DataModel, "d0");
        }

        return SqlTemplate
            .Replace("$$COLUMNS$$", Columns.Any() ? string.Join(",", Columns.Select(x => $"d0.{x.ColumnName}")) : "*")
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$JOINS$$", _joins.RenderJoins())
            .Replace("$$FILTERS$$", Filters.Any() ? Filters.RenderWhereClause() : string.Empty)
            .Replace("$$ORDERBYCOLUMNS$$", string.Empty);
    }

    public override IEnumerable<TDataModel> Execute()
    {
        if (Connection.State != ConnectionState.Open)
        {
            Connection.Open();
        }

        List<DbParameter> filterParameters = [];
        Filters.ForEach(x => filterParameters.AddRange(x.Parameters));

        var parameterDictionary = new Dictionary<string, object>();
        filterParameters.ForEach(x => parameterDictionary.Add(x.ParameterName.Substring(1), x.Value));

        var sql = RenderSqlCommand();
        Console.WriteLine(sql);

        Type[] types = [ typeof(TDataModel), .._joins.Select(x => x.RefPropertyInfo.PropertyType).ToArray() ];

        return Connection.Query(
            sql: sql,
            types: types,
            map: (objs) =>
            {
                var rootDataModel = (TDataModel)objs[0];

                // One to One relation
                for (int i = 0; i < _joins.Count; ++i)
                {
                    _joins[i].RefPropertyInfo.SetValue(obj: rootDataModel, value: objs[i + 1]);
                }

                // One to many
                // (...)

                // Many to many
                // (...)

                return rootDataModel;
            },
            param: parameterDictionary,
            transaction: Transaction,
            buffered: true,
            splitOn: "Id,Id", // skip the first (TDataModel), second id, third id
            commandTimeout: null,
            commandType: CommandType.Text);
    }
}

/*
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

class Program
{
    static void Main()
    {
        string connectionString = "YourConnectionStringHere";
        using (var connection = new SqlConnection(connectionString))
        {
            string sql = @"
            SELECT 
                o.OrderId, o.CustomerName, 
                p.ProductId, p.ProductName, p.OrderId
            FROM 
                Orders o
            INNER JOIN 
                Products p ON o.OrderId = p.OrderId";

            var orderDictionary = new Dictionary<int, Order>();

            var orders = connection.Query<Order, Product, Order>(
                sql,
                (order, product) =>
                {
                    if (!orderDictionary.TryGetValue(order.OrderId, out var currentOrder))
                    {
                        currentOrder = order;
                        orderDictionary.Add(currentOrder.OrderId, currentOrder);
                    }
                    currentOrder.Products.Add(product);
                    return currentOrder;
                },
                splitOn: "ProductId"
            ).Distinct().ToList();

            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer: {order.CustomerName}");
                foreach (var product in order.Products)
                {
                    Console.WriteLine($"    Product ID: {product.ProductId}, Name: {product.ProductName}");
                }
            }
        }
    }
}


*/
