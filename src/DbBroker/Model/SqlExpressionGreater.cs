using DbBroker.Model;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DbBroker;

/// <summary>
/// SQL Expression for Greater Than and Greater Than or Equal To operators
/// </summary>
public class SqlGreater : SqlExpression
{

    private string _operator;

    private SqlGreater(SqlOperator sqlOperator, object value) : base(sqlOperator)
    {
        if (value is not null)
        {
            Parameters = [value];
        }

        _operator = sqlOperator == SqlOperator.LessThan ? ">" : ">=";
    }

    // TODO include the ability to compare with another column

    /// <summary>
    /// Specifies the right side of the Greater Than Expression (x > y)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SqlGreater Than(object value)
    {
        return new SqlGreater(SqlOperator.GreaterThan, value);
    }

    /// <summary>
    /// Specifies the right side of the Greater Than or Equal To Expression (x >= y)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static SqlGreater ThanOrEqualTo(object value)
    {
        return new SqlGreater(SqlOperator.GreaterThanOrEqual, value);
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

        return $"{(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} {_operator} {parameters.FirstOrDefault()?.ParameterName}";
    }
}
