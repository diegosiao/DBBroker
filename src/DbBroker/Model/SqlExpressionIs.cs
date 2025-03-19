using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlIs : SqlExpression
{
    private readonly bool _isNull;

    private SqlIs(SqlOperator sqlOperator, bool isNull) : base(sqlOperator)
    {
        _isNull = isNull;
        Parameters = [];
    }

    public static SqlIs Null()
    {
        return new SqlIs(SqlOperator.Is, true);
    }

    public static SqlIs NotNull()
    {
        return new SqlIs(SqlOperator.Is, false);
    }

    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} IS {(_isNull ? string.Empty : "NOT")} NULL";
    }
}
