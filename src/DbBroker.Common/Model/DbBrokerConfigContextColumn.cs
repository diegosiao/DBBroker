using System;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContextColumn
{
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public string SchemaName { get; set; } = string.Empty;

    public string TableName { get; set; }

    public string ViewName { get; set; }

    public string ColumnName { get; set; }

    public string DataType { get; set; }

    public string MaxLength { get; set; }

    public string DataTypePrecision { get; set; }

    public string DataTypeScale { get; set; }

    public bool IsNullable { get; set; }

    // TODO the logic to deal with read only column is missing
    public bool ReadOnly { get; set; }

    public string ColumnFullName => $"{SchemaName}.{TableName}{ViewName}.{ColumnName}";
}
