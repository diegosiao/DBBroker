using System.Collections.Generic;
using System.Data.Common;

namespace DbBroker.Model;

public class CommandFilter
{
    public DataModelMapProperty DataModelMapProperty { get; set; }

    public int Index { get; set; }

    public SqlExpression SqlExpression { get; set; }

    public IEnumerable<DbParameter> Parameters { get; set; }

    public string Alias { get; set; }

    public string RenderSql()
    {
        return SqlExpression.RenderSql(Alias, DataModelMapProperty.ColumnName, Parameters, 0);
    }
}
