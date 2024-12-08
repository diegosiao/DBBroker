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

    private List<SqlJoin> _joins = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _load = [];

    private readonly IEnumerable<Expression<Func<TDataModel, object>>> _ignore = [];

    private readonly List<SqlOrderBy<TDataModel>> _orderBy = [];

    internal
    SqlSelectCommand(
        TDataModel dataModel,
        DbConnection connection,
        IEnumerable<Expression<Func<TDataModel, object>>> load = null,
        IEnumerable<Expression<Func<TDataModel, object>>> ignore = null,
        DbTransaction transaction = null,
        int depth = 0,
        int skip = 0,
        int take = 0) :
        base(dataModel, columns: [], parameters: [], connection, transaction, Constants.SqlSelectTemplate)
    {
        _load = load ?? [];
        _ignore = ignore ?? [];
        _maxDepth = depth;
        _skip = skip;
        _take = take;
    }

    public SqlSelectCommand<TDataModel> OrderBy(Expression<Func<TDataModel, object>> property, bool ascending = true)
    {
        _orderBy.Add(ascending ? SqlOrderBy<TDataModel>.Ascending(property) : SqlOrderBy<TDataModel>.Descending(property));
        return this;
    }

    int joinIndex = 0;
    private void LoadJoins(int depth, IDataModel dataModel, bool parentIsCollection, string parentAliasName, string path)
    {
        if (depth > _maxDepth)
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
                join.Depth + 1,
                referenceInstance,
                parentIsCollection: false,
                join.RefTableNameAlias,
                join.Path);
        }

        if (depth != 1)
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
            if (!_load.Any(x => (x.Body as MemberExpression)?.Member?.Name?.Equals(reference.Value.PropertyInfo.Name) ?? false))
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
            LoadJoins(1, DataModel, parentIsCollection: false, "d0", null);
            _joins = [.. _joins.OrderBy(x => x.Index)];
        }

        // prepare filters for a select
        Filters.ForEach(x => x.Alias = "d0");

        return SqlTemplate
            .Replace("$$COLUMNS$$", RenderColumns())
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$JOINS$$", _joins.RenderJoins())
            .Replace("$$FILTERS$$", Filters.Any() ? Filters.RenderWhereClause() : string.Empty)
            .Replace("$$ORDERBYCOLUMNS$$", RenderOrderByColumns())
            .Replace("$$OFFSETFETCH$$", _offsetFetchSql);
    }

    private string RenderOrderByColumns()
    {
        if (_orderBy.Count == 0)
        {
            return string.Empty;
        }

        var orderByColumns = new List<string>();

        foreach (var column in _orderBy)
        {
            var propertyPath = PropertyPathHelper.GetNestedPropertyPath(column.Expression);
            var propertyPathSplitted = propertyPath.Split('.');
            if (propertyPathSplitted.Count() == 1)
            {
                orderByColumns.Add($"d0.{DataModel.DataModelMap.MappedProperties[propertyPath].ColumnName} {(IsOrderByAscending(propertyPath) ? "ASC" : "DESC")}");   
                continue;
            }

            var join = _joins.FirstOrDefault(x => x.Path.Equals(string.Join('.', propertyPathSplitted.SkipLast(1))));

            if (join == null)
            {
                continue;
            }

            var mappedProperty = join
                .DataModelMap
                .MappedProperties[propertyPathSplitted.TakeLast(1).First()];

            orderByColumns.Add($"{join.RefTableNameAlias}.{mappedProperty.ColumnName} {(IsOrderByAscending($"{join.Path}.{mappedProperty.PropertyName}") ? "ASC" : "DESC")}");
        }

        return $"ORDER BY {string.Join(',', orderByColumns)}";
    }

    private bool IsOrderByAscending(string propertyPath)
    {
        return _orderBy.FirstOrDefault(c => PropertyPathHelper.GetNestedPropertyPath(c.Expression).Equals(propertyPath))?.Asc ?? true;
    }

    private string RenderColumns()
    {
        var sqlSelectColumns = new List<string>();

        // Data model included columns
        var includedRootProperties = _load
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

        // Only the ones affecting joins
        var includedReferencesProperties = _load
            .Select(x => PropertyPathHelper.GetNestedPropertyPath(x))
            .Where(x => x.Split('.').Length > 1);

        foreach (var join in _joins)
        {
            var joinIncludedProperties = includedReferencesProperties
                                    .Where(p => string.Join('.', p.Split('.').SkipLast(1)).Equals(join.Path));

            // Check if the depth and reference has load properties to restrict the columns
            // or load them all
            sqlSelectColumns.AddRange(join
                    .DataModelMap
                    .MappedProperties
                    .Where(x => joinIncludedProperties.Any() ?
                                    joinIncludedProperties.Contains($"{join.Path}.{x.Value.PropertyName}") || x.Value.IsKey
                                    : true)
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
                    var join = _joins[i];
                    join.TransientRef = objs[i + 1];

                    if (join.IsCollection)
                    {
                        var collectionValue = join.RefPropertyInfo.GetValue(rootDataModel, null);

                        if (collectionValue == null)
                        {
                            var collectionType = typeof(List<>).MakeGenericType(join.RefPropertyCollectionType);
                            collectionValue = Activator.CreateInstance(collectionType);

                            join.RefPropertyInfo.SetValue(obj: rootDataModel, value: collectionValue);
                        }

                        if (join.TransientRef is null)
                        {
                            continue;
                        }

                        var joinTransientRefDataModel = (IDataModel)join.TransientRef;
                        var transientRefKeyValue = joinTransientRefDataModel.DataModelMap.KeyProperty.GetValue(joinTransientRefDataModel, null);

                        if (!join.MapBuffer.ContainsKey(transientRefKeyValue))
                        {
                            join.MapBuffer.Add(transientRefKeyValue, join.TransientRef);
                            ((IList)collectionValue).Add(join.TransientRef);
                        }
                        continue;
                    }

                    if (objs[i + 1] == null)
                    {
                        continue;
                    }

                    var _targetJoin = _joins.Find(x => x.RefTableNameAlias.Equals(join.ParentTableAliasName));
                    join.RefPropertyInfo.SetValue(obj: _targetJoin?.TransientRef ?? rootDataModel, value: objs[i + 1]);
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
