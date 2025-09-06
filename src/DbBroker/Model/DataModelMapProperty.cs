using System.Reflection;

namespace DbBroker.Model;

/// <summary>
/// Represents a mapping between a property in a data model and a column in a database table.
/// </summary>
public class DataModelMapProperty
{
    /// <summary>
    /// The schema name of the table containing the column.
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    /// The name of the property in the data model.
    /// </summary>
    public string PropertyName => PropertyInfo?.Name;

    /// <summary>
    /// Indicates whether the column is part of the primary key.
    /// </summary>
    public bool IsKey { get; set; }

    /// <summary>
    /// The index of the property in the collection of properties.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The database type of the column (e.g., SqlDbType for SQL Server).
    /// </summary>
    public object ProviderDbType { get; set; }

    /// <summary>
    /// The PropertyInfo of the property in the data model.
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }
}
