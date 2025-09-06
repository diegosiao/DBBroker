using System.Reflection;

namespace DbBroker.Model;

/// <summary>
/// Data model map reference.
/// </summary>
public class DataModelMapReference
{
    /// <summary>
    /// Referencing schema name.
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// Referencing table name.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Referencing column name.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// Indicates whether the column allows nulls.
    /// </summary>
    public bool ColumnAllowNulls { get; set; }

    /// <summary>
    /// Referenced schema name.
    /// </summary>
    public string RefSchemaName { get; set; }

    /// <summary>
    /// Referenced table name.
    /// </summary>
    public string RefTableName { get; set; }

    /// <summary>
    /// Referenced table primary key column name.
    /// </summary>
    public string RefTablePrimaryKeyColumnName { get; set; }

    /// <summary>
    /// Referenced column name.
    /// </summary>
    public string RefColumnName { get; set; }

    /// <summary>
    /// Index of the reference in the data model map's References collection.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Property info of the referencing property.
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }
}
