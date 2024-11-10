using System;
using System.Collections.Generic;
using System.Text;
using DbBroker.Model;

namespace DbBroker.Extensions;

public static class SqlRenderExtensions
{
    public static string RenderWhereClause(this IEnumerable<CommandFilter> filters)
    {
        var whereClause = new StringBuilder();
        foreach (var filter in filters)
        {
            whereClause.AppendLine($"AND {filter.SqlExpression.RenderSql(filter.DataModelMapProperty.ColumnName, filter.Index)}");
        }
        return whereClause.ToString();
    }
}
