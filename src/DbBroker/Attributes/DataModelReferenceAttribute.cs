using System;

namespace DbBroker.Attributes;

public class DataModelReferenceAttribute : Attribute
{
    public string Property { get; private set; }

    public string SchemaName { get; set; }

    public string TableName { get; set; }

    public string ColumnName { get; private set; }

    public bool ColumnAllowNulls { get; set; }

    public string RefColumnName { get; private set; }

    public string RefSchemaName { get; private set; }

    public string RefTableName { get; private set; }

    public string RefTableFullName => $"{RefSchemaName}.{RefTableName}";

    public DataModelReferenceAttribute(
        string property, 
        string schemaName, 
        string tableName, 
        string columnName, 
        bool columnAllowNulls,
        string refColumnName, 
        string refSchemaName, 
        string refTableName)
    {
        Property = property;
        SchemaName = schemaName;
        TableName = tableName;
        ColumnName = columnName;
        ColumnAllowNulls = columnAllowNulls;
        RefColumnName = refColumnName;
        RefSchemaName = refSchemaName;
        RefTableName = refTableName;
    }
}
