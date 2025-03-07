using System;

namespace DbBroker.Attributes;

public class DataModelCollectionReferenceAttribute : DataModelReferenceBaseAttribute
{
    public Type DataModelType { get; set; }

    public string RefTablePrimaryKeyColumnName { get; set; }

    public DataModelCollectionReferenceAttribute(string splitOnColumnName, Type dataModelType)
    {
        DataModelType = dataModelType;
        ColumnName = splitOnColumnName;
    }

    public DataModelCollectionReferenceAttribute(
        string schemaName,
        string tableName,
        string columnName,
        string refColumnName,
        string refSchemaName,
        string refTableName,
        string refTablePrimaryKeyColumnName,
        Type dataModelType)
    {
        SchemaName = schemaName;
        TableName = tableName;
        ColumnName = columnName;
        RefColumnName = refColumnName;
        RefSchemaName = refSchemaName;
        RefTableName = refTableName;
        RefTablePrimaryKeyColumnName = refTablePrimaryKeyColumnName;
        DataModelType = dataModelType;
    }
}
