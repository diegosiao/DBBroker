using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// Represents a SQL "less than" or "less than or equal to" expression.
/// </summary>
public class SqlLess : SqlExpression
{
    private string _operator;

    private SqlLess(SqlOperator sqlOperator, object value) : base(sqlOperator)
    {
        if (value is not null)
        {
            Parameters = [value];
        }

        _operator = sqlOperator == SqlOperator.LessThan ? "<" : "<=";
    }

    // TODO include the ability to compare with another column

    /// <summary>
    /// Creates a "less than" SQL expression.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SqlLess Than(object value)
    {
        return new SqlLess(SqlOperator.LessThan, value);
    }

    /// <summary>
    /// Creates a "less than or equal to" SQL expression.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SqlLess ThanOrEqualTo(object value)
    {
        return new SqlLess(SqlOperator.LessThanOrEqual, value);
    }

    /// <summary>
    /// Renders the SQL representation of the expression.
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

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} {_operator} {parameters.FirstOrDefault()?.ParameterName}";
    }
}
