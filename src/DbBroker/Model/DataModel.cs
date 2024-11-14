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

    public DataModelMap DataModelMap
    {
        get
        {
            _dataModelMap ??= GetDataModelMap();
            return _dataModelMap;
        }
    }

    /// <summary>
    /// Meta column to represent '*' in SQL SELECT commands
    /// </summary>
    [DataModelMetaColumn]
    public dynamic ALL { get; set; }

    protected Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsNotPristine(string propertyName) => _IsNotPristine.ContainsKey(propertyName);

    internal IEnumerable<PropertyInfo> GetCachedProperties() => Properties;

    protected static string ISqlInsertTemplateTypeFullName;

    protected static SupportedDatabaseProviders Provider;

    readonly string[] _internalProperties = ["ALL", "DataModelMap"];

    private DataModelMap GetDataModelMap()
    {
        DbBroker.SqlInsertTemplates.TryGetValue(ISqlInsertTemplateTypeFullName, out ISqlInsertTemplate sqlInsertTemplate);

        if (sqlInsertTemplate == null)
        {
            sqlInsertTemplate = Activator.CreateInstance(Type.GetType(ISqlInsertTemplateTypeFullName)) as ISqlInsertTemplate;
            DbBroker.SqlInsertTemplates.Add(ISqlInsertTemplateTypeFullName, sqlInsertTemplate);
        }

        DataModelMap dataModelMap = new()
        {
            SchemaName = typeof(T).GetCustomAttribute<TableAttribute>().Schema,
            TableName = typeof(T).GetCustomAttribute<TableAttribute>().Name,
            Provider = Provider,
            SqlInsertTemplate = sqlInsertTemplate,
        };

        var index = 0;
        foreach (var property in Properties)
        {
            if (_internalProperties.Contains(property.Name))
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

            // Debug.WriteLine(property.Name);
            var dataModelProperty = new DataModelMapProperty()
            {
                ColumnName = property.GetCustomAttribute<ColumnAttribute>().Name,
                IsKey = property.GetCustomAttribute<KeyAttribute>() != null,
                Index = index++,
                PropertyInfo = property
            };

            dataModelMap.MappedProperties.Add(property.Name, dataModelProperty);
        }

        return dataModelMap;
    }
}
