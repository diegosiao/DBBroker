using System;
using System.Collections.Generic;
using System.Reflection;

namespace DbBroker.Model;

public class SqlJoin
{
    public int Depth { get; set; }

    public int Index { get; set; }

    public string Path { get; set; }

    public DataModelMap DataModelMap { get; set; }

    public string SchemaName { get; set; }

    public string ParentTableName { get; set; }

    public string ParentTablePrimaryKeyColumnName { get; set; }

    public string ParentTableAliasName { get; set; }

    public string ParentColumnName { get; set; }

    public bool ColumnAllowNulls { get; set; }

    public string RefTableName { get; set; }

    public string RefColumnName { get; set; }

    public string RefTableNameAlias => $"d{Depth}_j{Index}";

    public PropertyInfo RefPropertyInfo { get; set; }

    public Type RefPropertyCollectionType { get; set; }

    public object TransientRef { get; set; }

    public Dictionary<object, object> MapBuffer { get; set; } = new Dictionary<object, object>(comparer: new DataModelKeyComparer());

    public bool IsCollection => RefPropertyCollectionType is not null;

    public bool ParentIsCollection { get; set; }
}
