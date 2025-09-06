using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbBroker.Model;

namespace DbBroker.Extensions;

/// <summary>
/// Provides extension methods for SQL rendering
/// </summary>
public static class SqlRenderExtensions
{
    /// <summary>
    /// Renders the WHERE clause from a list of <see cref="CommandFilter" />.
    /// <para>Each filter is combined using AND operator.</para>
    /// <para>If the list is empty or null, an empty string is returned.</para>
    /// </summary>
    /// <param name="filters"></param>
    /// <returns></returns>
    public static string RenderWhereClause(this IEnumerable<CommandFilter> filters)
    {
        if (filters is null || !filters.Any())
        {
            return string.Empty;
        }

        var whereClause = new StringBuilder();
        foreach (var filter in filters)
        {
            var filterSql = filter.RenderSql();

            if (string.IsNullOrEmpty(filterSql))
                continue;

            whereClause.AppendLine(filterSql);
        }
        return whereClause.ToString();
    }

    /// <summary>
    /// Renders the SELECT columns from a list of <see cref="SqlJoin" />.
    /// <para>If the list is empty or null, an empty string is returned.</para>
    /// </summary>
    /// <param name="joins"></param>
    /// <returns></returns>
    public static string RenderJoinsColumns(this IEnumerable<SqlJoin> joins)
    {
        return string.Empty;
    }

    /// <summary>
    /// Renders the JOIN clauses from a list of <see cref="SqlJoin" />.
    /// <para>If the list is empty or null, an empty string is returned.</para>
    /// </summary>
    /// <param name="joins"></param>
    /// <returns></returns>
    public static string RenderJoins(this IEnumerable<SqlJoin> joins)
    {
        StringBuilder joinsClause = new();
        foreach (var join in joins)
        {
            joinsClause.Append(@$"{(join.ColumnAllowNulls ? "LEFT " : string.Empty)}JOIN {join.SchemaName}.{join.RefTableName} {join.RefTableNameAlias} ");
            joinsClause.AppendLine(@$"ON {join.RefTableNameAlias}.{join.RefColumnName} = {join.ParentTableAliasName}.{join.ParentColumnName}");
        }
        return joinsClause.ToString();
    }
}
