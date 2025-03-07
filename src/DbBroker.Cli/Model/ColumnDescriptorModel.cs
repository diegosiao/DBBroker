using DbBroker.Common.Model;

namespace DbBroker.Cli.Model;

public class ColumnDescriptorModel : DbBrokerConfigContextColumn
{
    public int Index { get; set; }
}
