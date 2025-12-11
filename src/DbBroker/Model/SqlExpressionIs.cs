using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL Expression for IS NULL and IS NOT NULL operators
/// </summary>
public class SqlIs : SqlExpression
{
    private readonly bool _isNull;

    private SqlIs(SqlOperator sqlOperator, bool isNull) : base(sqlOperator)
    {
        _isNull = isNull;
        Parameters = [];
    }

    /// <summary>
    /// Specifies the IS NULL expression
    /// </summary>
    /// <returns></returns>
    public static SqlIs Null()
    {
        return new SqlIs(SqlOperator.Is, true);
    }

    /// <summary>
    /// Specifies the IS NOT NULL expression
    /// </summary>
    /// <returns></returns>
    public static SqlIs NotNull()
    {
        return new SqlIs(SqlOperator.Is, false);
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
        return $"{(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} IS {(_isNull ? string.Empty : "NOT")} NULL";
    }
}
