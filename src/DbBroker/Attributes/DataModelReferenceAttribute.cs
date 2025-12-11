namespace DbBroker.Attributes;

/// <summary>
/// Indicates that a property is a reference to another data model.
/// </summary>
public class DataModelReferenceAttribute : DataModelReferenceBaseAttribute
{
    /// <summary>
    /// The property that holds the reference.
    /// </summary>
    public string Property { get; private set; }

    /// <summary>
    /// The name of the schema that contains the table with the foreign key column.
    /// </summary>
    public bool ColumnAllowNulls { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataModelReferenceAttribute"/> class.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="schemaName"></param>
    /// <param name="tableName"></param>
    /// <param name="columnName"></param>
    /// <param name="columnAllowNulls"></param>
    /// <param name="refColumnName"></param>
    /// <param name="refSchemaName"></param>
    /// <param name="refTableName"></param>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="DataModelReferenceAttribute"/> class.
    /// </summary>
    /// <param name="splitOnColumnName"></param>
    public DataModelReferenceAttribute(string splitOnColumnName)
    {
        ColumnName = splitOnColumnName;
    }
}
