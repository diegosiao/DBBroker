using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbBroker.Model;

namespace DbBroker.Extensions;

public static class SqlRenderExtensions
{
    public static string RenderWhereClause(this IEnumerable<CommandFilter> filters)
    {
        if (filters is null || !filters.Any())
        {
            return "1=1";
        }

        var whereClause = new StringBuilder();
        foreach (var filter in filters)
        {
            whereClause.AppendLine($"AND {filter.RenderSql()}");
        }
        return whereClause.ToString();
    }

    public static string RenderJoinsColumns(this IEnumerable<SqlJoin> joins)
    {
        return string.Empty;
    }

    public static string RenderJoins(this IEnumerable<SqlJoin> joins)
    {
        StringBuilder joinsClause = new();
        foreach (var join in joins)
        {
            joinsClause.Append(@$"{(join.ColumnAllowNulls ? "LEFT " : string.Empty)}JOIN {join.SchemaName}.{join.RefTableName} {join.RefTableNameAlias} ");
            joinsClause.AppendLine(@$"ON {join.RefTableNameAlias}.{join.RefColumnName} = {join.TableAliasName}.{join.ColumnName}");
        }
        return joinsClause.ToString();
    }
}
