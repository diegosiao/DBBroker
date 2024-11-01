namespace DbBroker.Cli.Model;

public class TableDescriptorModel
{
    public required string SchemaName { get; set; }

    public required string TableName { get; set; }

    public string TableFullName => $"{SchemaName}.{TableName}";

    public IEnumerable<ColumnDescriptorModel> Columns { get; set; } = [];

    public IEnumerable<KeyDescriptorModel> Keys { get; set; } = [];
}
