using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DbBroker.Model;

/// <summary>
/// Represents a SQL Expression for NOT BETWEEN operator
/// </summary>
public class SqlExpressionNotBetween : SqlExpression
{
    private SqlExpressionNotBetween(object value1, object value2) : base(SqlOperator.NotBetween)
    {
        if (value1 is not null && value2 is not null)
        {
            Parameters = [value1, value2];
        }
    }

    // TODO add the ability to compare with another column

    /// <summary>
    /// Column specified NOT BETWEEN <paramref name="value1"/> AND <paramref name="value2"/>
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static SqlExpressionNotBetween These(object value1, object value2)
    {
        return new SqlExpressionNotBetween(value1, value2);
    }

    /// <summary>
    /// Renders the SQL for this expression
    /// </summary>
    /// <param name="alias"></param>
    /// <param name="columnName"></param>
    /// <param name="parameters"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        if (!Parameters.Any())
        {
            return string.Empty;
        }
        return $"{(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} NOT BETWEEN {parameters.First()?.ParameterName} AND {parameters.ElementAt(1)?.ParameterName}";
    }
}
