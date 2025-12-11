using System;
using System.Reflection;

namespace DbBroker.Model;

/// <summary>
/// Represents a reference from a data model to a collection of related data models.
/// </summary>
public class DataModelMapCollectionReference
{
    /// <summary>
    /// The schema name of the table containing the foreign key column.
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// The name of the table containing the foreign key column.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// The name of the foreign key column.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// The schema name of the referenced table.
    /// </summary>
    public string RefSchemaName { get; set; }

    /// <summary>
    /// The name of the referenced table.
    /// </summary>
    public string RefTableName { get; set; }

    /// <summary>
    /// The name of the primary key column in the referenced table.
    /// </summary>
    public string RefTablePrimaryKeyColumnName { get; set; }

    /// <summary>
    /// The name of the column in the referenced table that the foreign key points to.
    /// </summary>
    public string RefColumnName { get; set; }

    /// <summary>
    /// The index of the reference in the collection of references.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The PropertyInfo of the property in the data model that holds the collection of related data models.
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }

    /// <summary>
    /// The type of the data model that the collection references.
    /// </summary>
    public Type DataModelCollectionType { get; set; }
}
