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
    /// Index of the filter in the command's filter list
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// The SQL expression representing the filter condition
    /// </summary>
    public SqlExpression SqlExpression { get; set; }

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
        return SqlExpression.RenderSql(Alias, DataModelMapProperty.ColumnName, Parameters, 0);
    }
}
