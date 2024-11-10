using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using DbBroker.Common;
using DbBroker.Common.Model.Interfaces;
using DbBroker.Model;
using DBBroker.Model.Interfaces;

namespace DBBroker.Model;

public abstract class DataModelBase<T> : IDataModel
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

    protected Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsNotPristine(string propertyName) => _IsNotPristine.ContainsKey(propertyName);

    internal IEnumerable<PropertyInfo> GetCachedProperties() => Properties;

    protected static string ISqlInsertTemplateTypeFullName;

    protected static SupportedDatabaseProviders Provider;

    private DataModelMap GetDataModelMap()
    {
        DbBroker.DbBroker.SqlInsertTemplates.TryGetValue(ISqlInsertTemplateTypeFullName, out ISqlInsertTemplate sqlInsertTemplate);

        if (sqlInsertTemplate == null)
        {
            sqlInsertTemplate = Activator.CreateInstance(Type.GetType(ISqlInsertTemplateTypeFullName)) as ISqlInsertTemplate;
            DbBroker.DbBroker.SqlInsertTemplates.Add(ISqlInsertTemplateTypeFullName, sqlInsertTemplate);
        }

        DataModelMap dataModelMap = new()
        {
            TableName = typeof(T).GetCustomAttribute<TableAttribute>().Name,
            SchemaName = typeof(T).GetCustomAttribute<TableAttribute>().Schema,
            Provider = Provider,
            SqlInsertTemplate = sqlInsertTemplate
        };

        var index = 0;
        foreach (var property in Properties)
        {
            var dataModelProperty = new DataModelMapProperty()
            {
                ColumnName = property.GetCustomAttribute<ColumnAttribute>().Name,
                IsKey = property.GetCustomAttribute<KeyAttribute>() != null,
                Index = index++,
                PropertyInfo = property,
                PropertyName = property.Name
            };

            dataModelMap.MappedProperties.Add(property.Name, dataModelProperty);
        }

        return dataModelMap;
    }
}
