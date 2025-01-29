namespace DbBroker.Cli.Model;

public class KeyDescriptorModel
{
    public string SchemaName { get; set; } = string.Empty;

    public string TableName { get; set; } = string.Empty;

    public string TableFullName => $"{SchemaName}.{TableName}";

    public string ColumnName { get; set; } = string.Empty;

    public string ConstraintName { get; set; } = string.Empty;

    public string ConstraintType { get; set; } = string.Empty;

    public string ReferencedTable { get; set; } = string.Empty;

    public string ReferencedColumn { get;set; } = string.Empty;

    public string KeyFullName => $"{SchemaName}.{TableName}.{ColumnName}";
}
