using DbBroker.Model;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbBroker;

/// <summary>
/// Represents a SQL Join between two tables
/// </summary>
public class SqlJoin
{
    /// <summary>
    /// The depth of the join in the overall query
    /// </summary>
    public int Depth { get; set; }

    /// <summary>
    /// The index of the join at this depth
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The path to this join from the root data model
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// The DataModelMap for the referenced table
    /// </summary>
    public DataModelMap DataModelMap { get; set; }

    /// <summary>
    /// The schema name of the referenced table
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// The parent table name
    /// </summary>
    public string ParentTableName { get; set; }

    /// <summary>
    /// The primary key column name of the parent table
    /// </summary>
    public string ParentTablePrimaryKeyColumnName { get; set; }

    /// <summary>
    /// The alias name of the parent table
    /// </summary>
    public string ParentTableAliasName { get; set; }

    /// <summary>
    /// The parent column name that references the foreign key in the referenced table
    /// </summary>
    public string ParentColumnName { get; set; }

    /// <summary>
    /// The column name in the referenced table that is referenced by the foreign key in the parent table
    /// </summary>
    public bool ColumnAllowNulls { get; set; }

    /// <summary>
    /// The referenced table name
    /// </summary>
    public string RefTableName { get; set; }

    /// <summary>
    /// The referenced column name that is referenced by the foreign key in the parent table
    /// </summary>
    public string RefColumnName { get; set; }

    /// <summary>
    /// The alias name of the referenced table
    /// </summary>
    public string RefTableNameAlias => $"d{Depth}_j{Index}";

    /// <summary>
    /// The PropertyInfo of the property in the data model that represents this join
    /// </summary>
    public PropertyInfo RefPropertyInfo { get; set; }

    /// <summary>
    /// The type of the property in the data model that represents this join
    /// </summary>
    public Type RefPropertyCollectionType { get; set; }

    /// <summary>
    /// The transient reference object for this join
    /// </summary>
    public object TransientRef { get; set; }

    /// <summary>
    /// A buffer to hold already mapped objects to avoid circular references and duplicate objects
    /// </summary>
    public Dictionary<object, object> MapBuffer { get; set; } = new Dictionary<object, object>(comparer: new DataModelKeyComparer());

    /// <summary>
    /// Indicates if the property in the data model that represents this join is a collection
    /// </summary>
    public bool IsCollection => RefPropertyCollectionType is not null;

    /// <summary>
    /// Indicates if the parent property in the data model that represents this join is a collection
    /// </summary>
    public bool ParentIsCollection { get; set; }
}
