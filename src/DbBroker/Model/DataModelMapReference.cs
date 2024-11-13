using System;
using System.Reflection;

namespace DbBroker.Model;

public class DataModelMapReference
{
    public string SchemaName { get; set; }
    
    public string TableName { get; set; }

    public string ColumnName { get; set; }

    public bool ColumnAllowNulls { get; set; }

    public string RefSchemaName { get; set;}

    public string RefTableName { get; set; }

    public string RefColumnName { get; set; }

    public int Index { get; set; }

    public PropertyInfo PropertyInfo { get; set; }

    // public Type RefType { get; set; }
}
