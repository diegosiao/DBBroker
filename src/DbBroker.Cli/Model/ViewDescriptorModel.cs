namespace DbBroker.Cli.Model;

public class ViewDescriptorModel
{
    public required string SchemaName { get; set; }

    public required string ViewName { get; set; }

    public string ViewFullName => $"{SchemaName}.{ViewName}";

    public IEnumerable<ColumnDescriptorModel> Columns { get; set; } = [];
}
