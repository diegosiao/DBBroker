using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// Apply a filter using the equals to expression (x = y)
/// </summary>
public class SqlEquals : SqlExpression
{
    private bool _lowerAll;

    private SqlEquals(object value, bool lowerAll) : base(SqlOperator.Equals)
    {
        _lowerAll = lowerAll;

        if (value is not null)
        {
            Parameters = [value];
        }
    }

    /// <summary>
    /// Specifies the right side of the Equals Expression
    /// </summary>
    /// <param name="value">The value to compare to</param>
    /// <param name="lowerAll">If true, applies the SQL function lower() on both sides of the expression. Default value is false.</param>
    public static SqlEquals To(object value, bool lowerAll = false)
    {
        return new SqlEquals(value, lowerAll);
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

        if (_lowerAll)
        {
            return $"lower({(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName}) = lower({parameters.FirstOrDefault()?.ParameterName})";
        }

        return $"{(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} = {parameters.FirstOrDefault()?.ParameterName}";
    }
}
