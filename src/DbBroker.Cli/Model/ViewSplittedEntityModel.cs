using DbBroker.Common.Model;

namespace DbBroker.Cli.Model;

internal class ViewSplittedEntityModel
{
    public DbBrokerConfigContextViewSplitOnItem SplitOnItem { get; set; }

    public List<ColumnDescriptorModel> Columns { get; set; }

    public ViewSplittedEntityModel(DbBrokerConfigContextViewSplitOnItem splitOnItem)
    {
        SplitOnItem = splitOnItem;
        Columns = [];
    }
}
