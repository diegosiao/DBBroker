using System;

namespace DbBroker.Attributes;

public class DataModelReferenceAttribute : DataModelReferenceBaseAttribute
{
    public string Property { get; private set; }
    
    public bool ColumnAllowNulls { get; set; }

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

    public DataModelReferenceAttribute(string splitOnColumnName)
    {
        ColumnName = splitOnColumnName;
    }
}
