using System;

namespace DbBroker.Attributes;

/// <summary>
/// Specifies that a property is a reference to a collection of data models.
/// </summary>
public class DataModelCollectionReferenceAttribute : DataModelReferenceBaseAttribute
{
    /// <summary>
    /// Gets or sets the type of the data model in the collection.
    /// </summary>
    public Type DataModelType { get; set; }

    /// <summary>
    /// Gets or sets the name of the primary key column in the referenced table.
    /// </summary>
    public string RefTablePrimaryKeyColumnName { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataModelCollectionReferenceAttribute"/> class.
    /// </summary>
    /// <param name="splitOnColumnName"></param>
    /// <param name="dataModelType"></param>
    public DataModelCollectionReferenceAttribute(string splitOnColumnName, Type dataModelType)
    {
        DataModelType = dataModelType;
        ColumnName = splitOnColumnName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataModelCollectionReferenceAttribute"/> class.
    /// </summary>
    /// <param name="schemaName"></param>
    /// <param name="tableName"></param>
    /// <param name="columnName"></param>
    /// <param name="refColumnName"></param>
    /// <param name="refSchemaName"></param>
    /// <param name="refTableName"></param>
    /// <param name="refTablePrimaryKeyColumnName"></param>
    /// <param name="dataModelType"></param>
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
