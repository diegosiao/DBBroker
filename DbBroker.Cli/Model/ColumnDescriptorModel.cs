using System;

namespace DbBroker.Cli.Model;

public class ColumnDescriptorModel
{
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public string SchemaName { get; set; } = string.Empty;

    public required string TableName { get; set; }

    public required string ColumnName { get; set; }

    public required string DataType { get; set; }

    public string? MaxLength { get; set; }

    public bool IsNullable { get; set; }

    public string ColumnFullName => $"{SchemaName}.{TableName}.{ColumnName}";
}
