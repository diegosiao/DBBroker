using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// Represents a SQL "like" expression.
/// </summary>
public class SqlLike : SqlExpression
{
    private readonly bool _lowerAll;

    private SqlLike(object value, bool lowerAll) : base(SqlOperator.Equals)
    {
        _lowerAll = lowerAll;

        if (value is not null)
        {
            Parameters = [value];
        }
    }

    /// <summary>
    /// Creates a "like" SQL expression.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="lowerAll"></param>
    /// <returns></returns>
    public static SqlLike To(string value, bool lowerAll = true)
    {
        return new SqlLike(value, lowerAll);
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
            return $"AND lower({(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName}) LIKE lower({parameters.FirstOrDefault()?.ParameterName})";
        }

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} LIKE {parameters.FirstOrDefault()?.ParameterName}";
    }
}
