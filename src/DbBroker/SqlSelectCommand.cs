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

    private readonly int _skip;

    private readonly int _take;

    private string _offsetFetchSql => _take > 0 ? $"OFFSET {_skip} ROWS FETCH NEXT {_take} ROWS ONLY" : string.Empty;

    private List<SqlJoin> _joins;

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _include = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _orderByAsc = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _orderByDesc = [];

    internal
    SqlSelectCommand(
        TDataModel dataModel,
        DbConnection connection,
        IEnumerable<Expression<Func<TDataModel, object>>> include = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByAsc = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByDesc = null,
        DbTransaction transaction = null,
        int depth = 0,
        int skip = 0,
        int take = 0) :
        base(dataModel, columns: [], parameters: [], connection, transaction, Constants.SqlSelectTemplate)
    {
        _maxDepth = depth;
        _skip = skip;
        _take = take;
        _joins = [];
        _include = include ?? [];
        _orderByAsc = orderByAsc ?? [];
        _orderByDesc = orderByDesc ?? [];
    }

    int joinIndex = 0;
    private void LoadJoins(int depth, IDataModel dataModel, bool parentIsCollection, string parentAliasName, string path)
    {
        if (depth >= _maxDepth)
        {
            return;
        }

        foreach (var reference in dataModel.DataModelMap.MappedReferences)
        {
            // Skip circular references
            if (typeof(TDataModel) == reference.Value.PropertyInfo.PropertyType)
            {
                continue;
            }

            var referenceInstance = (IDataModel)Activator.CreateInstance(reference.Value.PropertyInfo.PropertyType);
            
            var join = new SqlJoin
            {
                Depth = depth,
                Index = ++joinIndex,
                Path = string.IsNullOrEmpty(path) ? reference.Value.PropertyInfo.Name : string.Join('.', [path, reference.Value.PropertyInfo.Name]),
                DataModelMap = referenceInstance.DataModelMap,
                SchemaName = reference.Value.SchemaName,
                ParentTableName = reference.Value.TableName,
                ParentColumnName = reference.Value.ColumnName,
                ColumnAllowNulls = depth > 0 || reference.Value.ColumnAllowNulls,
                ParentTableAliasName = parentAliasName,
                RefTableName = reference.Value.RefTableName,
                RefColumnName = reference.Value.RefColumnName,
                RefPropertyInfo = reference.Value.PropertyInfo,
                ParentIsCollection = parentIsCollection,
            };

            _joins.Add(join);
            LoadJoins(
                depth + 1,
                referenceInstance,
                parentIsCollection: false,
                join.RefTableNameAlias,
                join.Path);
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
                // Skip circular references
                continue;
            }

            // TODO Skip not in root collections 
            if (!_include.Any(x => (x.Body as MemberExpression)?.Member?.Name?.Equals(reference.Value.PropertyInfo.Name) ?? false))
            {
                // Skip not explicitly included collections
                continue;
            }

            var collectionReferenceDataModel = (IDataModel)Activator.CreateInstance(reference.Value.DataModelCollectionType);

            var join = new SqlJoin
            {
                Depth = depth,
                Index = ++joinIndex,
                Path = string.IsNullOrEmpty(path) ? reference.Value.PropertyInfo.Name : string.Join('.', [path, reference.Value.PropertyInfo.Name]),
                DataModelMap = collectionReferenceDataModel.DataModelMap,
                SchemaName = reference.Value.SchemaName,
                ParentTableName = reference.Value.TableName,
                ParentTablePrimaryKeyColumnName = reference.Value.RefTablePrimaryKeyColumnName,
                ParentColumnName = reference.Value.ColumnName,
                ColumnAllowNulls = true,
                ParentTableAliasName = parentAliasName,
                RefTableName = reference.Value.RefTableName,
                RefColumnName = reference.Value.RefColumnName,
                RefPropertyInfo = reference.Value.PropertyInfo,
                RefPropertyCollectionType = reference.Value.DataModelCollectionType
            };

            _joins.Add(join);
            LoadJoins(
                depth + 1,
                collectionReferenceDataModel,
                parentIsCollection: true,
                join.RefTableNameAlias,
                path: string.Empty);
        }
    }

    protected override string RenderSqlCommand()
    {
        if (_maxDepth > 0)
        {
            LoadJoins(0, DataModel, parentIsCollection: false, "d0", null);
            _joins = [.. _joins.OrderBy(x => x.Index)];
        }

        // prepare filters for a select
        Filters.ForEach(x => x.Alias = "d0");

        return SqlTemplate
            .Replace("$$COLUMNS$$", RenderColumns())
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$JOINS$$", _joins.RenderJoins())
            .Replace("$$FILTERS$$", Filters.Any() ? Filters.RenderWhereClause() : string.Empty)
            .Replace("$$ORDERBYCOLUMNS$$", string.Empty)
            .Replace("$$OFFSETFETCH$$", _offsetFetchSql);
    }

    private string RenderColumns()
    {
        var sqlSelectColumns = new List<string>();

        // Data model included columns
        var includedRootProperties = _include
            .Select(x => PropertyPathHelper.GetNestedPropertyPath(x))
            .Where(x =>
                x.Split('.').Length == 1
                && !DataModel.DataModelMap.MappedCollections.ContainsKey(x)
                && !DataModel.DataModelMap.MappedReferences.ContainsKey(x));

        sqlSelectColumns.AddRange(
            DataModel
                .DataModelMap
                .MappedProperties
                .Where(
                    x => x.Value.IsKey
                    || includedRootProperties.Count() == 0
                    || includedRootProperties.Any(p => p.Equals(x.Value.PropertyName)))
                .Select(x => $"d0.{x.Value.ColumnName} AS {x.Value.PropertyName}"));

        // Joins
        var includedReferences = _include
            .Select(x => PropertyPathHelper.GetNestedPropertyPath(x))
            .Where(x => x.Split('.').Length > 1);

        foreach (var join in _joins)
        {
            // TODO Check if the depth and reference has included properties to restrict the columns
            // ...

            // load them all
            sqlSelectColumns.AddRange(join
                    .DataModelMap
                    .MappedProperties
                    .Select(x => $"{join.RefTableNameAlias}.{x.Value.ColumnName} AS {x.Value.PropertyName}"));
        }

        return string.Join(',', sqlSelectColumns);
    }

    public override IEnumerable<TDataModel> Execute()
    {
        // TODO Check if there isn't invalid include expressions (duplicated, methods, collections not in root, etc.)

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

        var dataModelResultDictionary = new Dictionary<object, TDataModel>(comparer: new DataModelKeyComparer());
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

                    var _targetJoin = _joins.Find(x => x.RefTableNameAlias.Equals(_joins[i].ParentTableAliasName));
                    _joins[i].RefPropertyInfo.SetValue(obj: _targetJoin?.TransientRef ?? rootDataModel, value: objs[i + 1]);
                }

                return rootDataModel;
            },
            param: parameterDictionary,
            transaction: Transaction,
            buffered: true,
            splitOn: string.Join(',', _joins.Select(x => x.DataModelMap.KeyProperty.Name)),
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
