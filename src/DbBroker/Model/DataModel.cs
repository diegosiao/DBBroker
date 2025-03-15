using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Common.Model.Interfaces;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

public abstract class DataModel<T> : IDataModel
{
    private static IEnumerable<PropertyInfo> _properties;

    internal static IEnumerable<PropertyInfo> Properties
    {
        get
        {
            _properties ??= typeof(T).GetProperties();
            return _properties;
        }
    }

    private static DataModelMap _dataModelMap;

    internal DataModelMap DataModelMap
    {
        get
        {
            _dataModelMap ??= GetDataModelMap();
            return _dataModelMap;
        }
    }

    DataModelMap IDataModel.DataModelMap => DataModelMap;

    protected Dictionary<string, bool> _IsNotPristine { get; private set; } = [];

    public bool IsNotPristine(string propertyName) => _IsNotPristine.ContainsKey(propertyName);

    public void SetPristine(string propertyName)
    {
        if (_IsNotPristine.ContainsKey(propertyName))
        {
            _IsNotPristine.Remove(propertyName);
        }
    }

    protected static string SqlInsertTemplateTypeFullName;

    protected static object[] SqlInsertTemplateTypeArguments;

    protected static SupportedDatabaseProviders Provider;

    readonly string[] _ignoreProperties = ["ALL", "DataModelMap"];

    private DataModelMap GetDataModelMap()
    {
        // TODO Allow Views to INSERT, UPDATE, DELETE (using database view triggers)
        // var sqlInsertTemplate = Activator.CreateInstance(Type.GetType(SqlInsertTemplateTypeFullName), SqlInsertTemplateTypeArguments);

        DataModelMap dataModelMap = new()
        {
            SchemaName = typeof(T).GetCustomAttribute<TableAttribute>().Schema,
            TableName = typeof(T).GetCustomAttribute<TableAttribute>().Name,
            Provider = Provider,
            SqlInsertTemplate = this is IViewDataModel ?
                null
                : Activator.CreateInstance(Type.GetType(SqlInsertTemplateTypeFullName), SqlInsertTemplateTypeArguments) as ISqlInsertTemplate,
        };

        var index = 0;
        foreach (var property in Properties)
        {
            if (_ignoreProperties.Contains(property.Name))
            {
                continue;
            }

            if (property.GetCustomAttribute<KeyAttribute>() != null)
            {
                dataModelMap.KeyProperty = property;
            }

            // Object Reference
            var referenceAttribute = property.GetCustomAttribute<DataModelReferenceAttribute>();
            if (referenceAttribute is not null)
            {
                dataModelMap.MappedReferences.Add(property.Name, new DataModelMapReference
                {
                    SchemaName = dataModelMap.SchemaName,
                    TableName = dataModelMap.TableName,
                    ColumnName = referenceAttribute.ColumnName,
                    ColumnAllowNulls = referenceAttribute.ColumnAllowNulls,
                    RefSchemaName = referenceAttribute.RefSchemaName,
                    RefTableName = referenceAttribute.RefTableName,
                    RefColumnName = referenceAttribute.RefColumnName,
                    Index = index,
                    PropertyInfo = property
                });
                continue;
            }

            // Collection Reference
            var collectionAttribute = property.GetCustomAttribute<DataModelCollectionReferenceAttribute>();
            if (collectionAttribute is not null)
            {
                dataModelMap.MappedCollections.Add(property.Name, new DataModelMapCollectionReference
                {
                    SchemaName = dataModelMap.SchemaName,
                    TableName = dataModelMap.TableName,
                    ColumnName = collectionAttribute.ColumnName,
                    RefSchemaName = collectionAttribute.RefSchemaName,
                    RefTableName = collectionAttribute.RefTableName,
                    RefColumnName = collectionAttribute.RefColumnName,
                    Index = index,
                    PropertyInfo = property,
                    DataModelCollectionType = collectionAttribute.DataModelType,
                    RefTablePrimaryKeyColumnName = collectionAttribute.RefTablePrimaryKeyColumnName,
                });
                continue;
            }

            var dataModelProperty = new DataModelMapProperty()
            {
                ColumnName = property.GetCustomAttribute<ColumnAttribute>().Name,
                IsKey = property.GetCustomAttribute<KeyAttribute>() != null,
                Index = index++,
                PropertyInfo = property,
                ProviderDbType = property.GetCustomAttribute<ColumnType>().ProviderDbType
            };

            dataModelMap.MappedProperties.Add(property.Name, dataModelProperty);
        }

        return dataModelMap;
    }
}
