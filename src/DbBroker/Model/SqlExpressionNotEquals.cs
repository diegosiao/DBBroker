using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// Represents a SQL "not equals" expression.
/// </summary>
public class SqlNotEquals : SqlExpression
{
    private bool _lowerAll;

    private SqlNotEquals(object value, bool lowerAll) : base(SqlOperator.Equals)
    {
        _lowerAll = lowerAll;

        if (value is not null)
        {
            Parameters = [value];
        }
    }

    /// <summary>
    /// Specifies the right side of the Not Equals Expression
    /// </summary>
    /// <param name="value">The value to compare to</param>
    /// <param name="lowerAll">If true, applies the SQL function lower() on both sides of the expression. Default value is false.</param>
    public static SqlNotEquals To(object value, bool lowerAll = false)
    {
        return new SqlNotEquals(value, lowerAll);
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

        if (_lowerAll)
        {
            return $"AND lower({(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName}) <> lower({parameters.FirstOrDefault()?.ParameterName})";
        }

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} <> {parameters.FirstOrDefault()?.ParameterName}";
    }
}
