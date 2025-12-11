using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContextView
{
    public string Name { get; set; }

    public string TypeName { get; set; }

    public IEnumerable<DbBrokerConfigContextViewSplitOnItem> SplitsOn { get; set; } = [];
}
