using System;
using System.Reflection;

namespace DbBroker.Model;

public class SqlJoin
{
    public int Depth { get; set; }

    public int Index { get; set; }

    public string SchemaName { get; set; }

    public string TableName { get; set; }

    public string TablePrimaryKeyColumnName { get; set; }

    public string TableAliasName { get; set; }

    public string ColumnName { get; set; }

    public bool ColumnAllowNulls { get; set; }

    public string RefTableName { get; set; }

    public string RefColumnName { get; set; }

    public string RefTableNameAlias => $"d{Depth}_j{Index}_{RefTableName}";

    public string RefColumnNameAlias => $"{RefTableNameAlias}_{RefColumnName}";

    public PropertyInfo RefPropertyInfo { get; set; }

    public Type RefPropertyCollectionType { get; set; }

    public object TransientRef { get; set; }

    public bool IsCollection => RefPropertyCollectionType is not null;
}
