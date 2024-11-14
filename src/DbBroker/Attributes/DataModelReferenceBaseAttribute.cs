using System;

namespace DbBroker.Attributes;

public abstract class DataModelReferenceBaseAttribute : Attribute
{
    public string SchemaName { get; set; }

    public string TableName { get; set; }

    public string ColumnName { get; protected set; }

    public string RefColumnName { get; protected set; }

    public string RefSchemaName { get; protected set; }

    public string RefTableName { get; protected set; }

    public string RefTableFullName => $"{RefSchemaName}.{RefTableName}";
}
