using System.Collections.Generic;
using System.Data.Common;

namespace DbBroker.Model;

/// <summary>
/// Represents a filter applied to a SQL command
/// </summary>
public class CommandFilter
{
    /// <summary>
    /// The property of the data model being filtered
    /// </summary>
    public DataModelMapProperty DataModelMapProperty { get; set; }

    /// <summary>
    /// Index of the filter in the command's filter list. -1 indicates grouped filter.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Used to chain multiple filters together (AND, OR)
    /// </summary>
    public SqlOperator Operator { get; set; }

    /// <summary>
    /// The SQL expression representing the filter condition
    /// </summary>
    public SqlExpression SqlExpression { get; set; }

    /// <summary>
    /// The chained SQL expressions for the filter
    /// </summary>
    public List<CommandFilter> GroupedExpressions { get; set; } = [];

    /// <summary>
    /// Parameters associated with the filter
    /// </summary>
    public IEnumerable<DbParameter> Parameters { get; set; }

    /// <summary>
    /// Alias for the data model in the SQL command
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Renders the SQL representation of the filter
    /// </summary>
    /// <returns></returns>
    public string RenderSql()
    {
        var sql = SqlExpression.RenderSql(Alias, DataModelMapProperty.ColumnName, Parameters, 0);

        if (GroupedExpressions.Count == 0)
        {
            return $"AND {sql}";
        }

        foreach (var groupedExpression in GroupedExpressions)
        {
            var groupedSql = groupedExpression.SqlExpression.RenderSql(Alias, groupedExpression.DataModelMapProperty.ColumnName, groupedExpression.Parameters, 0);
            
            if (!string.IsNullOrEmpty(groupedSql))
            {
                sql += $" {groupedExpression.Operator} {groupedSql}";
            }
        }
        sql = $"AND ({sql})";
        
        return sql;
    }
}
