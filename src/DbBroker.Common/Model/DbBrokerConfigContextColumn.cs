using System;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContextColumn
{
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public string SchemaName { get; set; } = string.Empty;

    public string TableName { get; set; }

    public string ColumnName { get; set; }

    public string DataType { get; set; }

    public string MaxLength { get; set; }

    public bool IsNullable { get; set; }

    public string ColumnFullName => $"{SchemaName}.{TableName}.{ColumnName}";
}
