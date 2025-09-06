using System;

namespace DbBroker.Attributes;

/// <summary>
/// Base attribute for data model references (foreign keys).
/// </summary>
public abstract class DataModelReferenceBaseAttribute : Attribute
{
    /// <summary>
    /// Schema name of the table containing the foreign key.
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// Table name containing the foreign key.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Column name of the foreign key.
    /// </summary>
    public string ColumnName { get; protected set; }

    /// <summary>
    /// Column name of the referenced primary key.
    /// </summary>
    public string RefColumnName { get; protected set; }

    /// <summary>
    /// Schema name of the referenced table.
    /// </summary>
    public string RefSchemaName { get; protected set; }

    /// <summary>
    /// Table name of the referenced table.
    /// </summary>
    public string RefTableName { get; protected set; }

    /// <summary>
    /// Full name of the table containing the foreign key.
    /// </summary>
    public string RefTableFullName => $"{RefSchemaName}.{RefTableName}";
}
