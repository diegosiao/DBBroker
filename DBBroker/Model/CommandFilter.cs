using System.Collections.Generic;

namespace DbBroker.Model;

public class CommandFilter
{
    public DataModelMapProperty DataModelMapProperty { get; set; }

    public int Index { get; set; }

    public SqlExpression SqlExpression { get; set; }
}
