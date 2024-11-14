using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DbBroker.Extensions;
using DbBroker.Model;
using Dapper;
using System;
using DbBroker.Model.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Collections;

namespace DbBroker;

public class SqlSelectCommand<TDataModel> : SqlCommand<TDataModel, IEnumerable<TDataModel>> where TDataModel : DataModel<TDataModel>
{
    private readonly int _maxDepth;

    private List<SqlJoin> _joins;

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _include = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _orderByAsc = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _orderByDesc = [];

    internal SqlSelectCommand(
        TDataModel dataModel,
        DbConnection connection,
        IEnumerable<Expression<Func<TDataModel, object>>> include = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByAsc = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByDesc = null,
        DbTransaction transaction = null,
        int depth = 0) :
        base(dataModel, columns: [], parameters: [], connection, transaction, Constants.SqlSelectTemplate)
    {
        _maxDepth = depth;
        _joins = [];
        _include = include ?? [];
        _orderByAsc = orderByAsc ?? [];
        _orderByDesc = orderByDesc ?? [];
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
            if (typeof(TDataModel) == reference.Value.PropertyInfo.PropertyType)
            {
                continue;
            }

            var join = new SqlJoin
            {
                Depth = depth,
                Index = ++joinIndex,
                SchemaName = reference.Value.SchemaName,
                TableName = reference.Value.TableName,
                ColumnName = reference.Value.ColumnName,
                ColumnAllowNulls = depth > 0 || reference.Value.ColumnAllowNulls,
                TableAliasName = parentAliasName,
                RefTableName = reference.Value.RefTableName,
                RefColumnName = reference.Value.RefColumnName,
                RefPropertyInfo = reference.Value.PropertyInfo
            };

            _joins.Add(join);
            LoadJoins(depth + 1, (IDataModel)Activator.CreateInstance(reference.Value.PropertyInfo.PropertyType), join.TableAliasName);
        }

        if (depth != 0)
        {
            // Only collections at the Data Model root can be included
            return;
        }

        foreach (var reference in dataModel.DataModelMap.MappedCollections)
        {
            if (typeof(TDataModel) == reference.Value.DataModelCollectionType)
            {
                continue;
            }

            var join = new SqlJoin
            {
                Depth = depth,
                Index = ++joinIndex,
                SchemaName = reference.Value.SchemaName,
                TableName = reference.Value.TableName,
                TablePrimaryKeyColumnName = reference.Value.RefTablePrimaryKeyColumnName,
                ColumnName = reference.Value.ColumnName,
                ColumnAllowNulls = true,
                TableAliasName = parentAliasName,
                RefTableName = reference.Value.RefTableName,
                RefColumnName = reference.Value.RefColumnName,
                RefPropertyInfo = reference.Value.PropertyInfo,
                RefPropertyCollectionType = reference.Value.DataModelCollectionType
            };

            _joins.Add(join);
            LoadJoins(depth + 1, (IDataModel)Activator.CreateInstance(reference.Value.DataModelCollectionType), join.RefTableNameAlias);
        }
    }

    protected override string RenderSqlCommand()
    {
        if (_maxDepth > 0)
        {
            LoadJoins(0, DataModel, "d0");
            _joins = [.. _joins.OrderBy(x => x.TableAliasName)];
        }

        // prepare filters for a select
        Filters.ForEach(x => x.Alias = "d0");

        return SqlTemplate
            .Replace("$$COLUMNS$$", _joins.Any() ? string.Join(",", ["d0.*", .. _joins.Select(x => $"{x.RefTableNameAlias}.*")]) : "*")
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
        filterParameters.ForEach(x => parameterDictionary.Add(x.ParameterName[1..], x.Value));

        var sql = RenderSqlCommand();

        Debug.WriteLine(sql);

        Type[] types = [
            typeof(TDataModel), 
            .. _joins.Select(x => x.RefPropertyCollectionType ?? x.RefPropertyInfo.PropertyType).ToArray(),
        ];

        var splitOn = string.Join(",", _joins.Select(x => x.IsCollection ? x.TablePrimaryKeyColumnName : x.RefColumnName));

        var dataModelResultDictionary = new Dictionary<object, TDataModel>();
        return Connection.Query(
            sql: sql,
            types: types,
            map: (objs) =>
            {
                var keyValue = ((TDataModel)objs[0]).DataModelMap.KeyProperty.GetValue(objs[0], null);
                if (!dataModelResultDictionary.TryGetValue(keyValue, out TDataModel rootDataModel))
                {
                    dataModelResultDictionary.Add(keyValue, rootDataModel = (TDataModel)objs[0]);
                }
                
                // Load mapped joins
                for (int i = 0; i < _joins.Count; ++i)
                {
                    _joins[i].TransientRef = objs[i + 1];

                    if (_joins[i].IsCollection)
                    {
                        var collectionValue = _joins[i].RefPropertyInfo.GetValue(rootDataModel, null);

                        if (collectionValue == null)
                        {
                            var collectionType = typeof(List<>).MakeGenericType(_joins[i].RefPropertyCollectionType);
                            collectionValue = Activator.CreateInstance(collectionType);

                            _joins[i].RefPropertyInfo.SetValue(obj: rootDataModel, value: collectionValue);
                        }

                        if (_joins[i].TransientRef is null)
                        {
                            continue;
                        }

                        ((IList)collectionValue).Add(objs[i + 1]);
                        continue;
                    }

                    if (objs[i + 1] == null)
                    {
                        continue;
                    }

                    var _targetJoin = _joins.Find(x => x.RefTableNameAlias.Equals(_joins[i].TableAliasName));
                    _joins[i].RefPropertyInfo.SetValue(obj: _targetJoin?.TransientRef ?? rootDataModel, value: objs[i + 1]);
                }

                return rootDataModel;
            },
            param: parameterDictionary,
            transaction: Transaction,
            buffered: false,
            splitOn: splitOn, // skip the first (TDataModel), second id, third id
            commandTimeout: null,
            commandType: CommandType.Text)
            .Distinct();
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
