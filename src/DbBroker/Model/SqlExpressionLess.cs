using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

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

    public static SqlLess Than(object value)
    {
        return new SqlLess(SqlOperator.LessThan, value);
    }

    public static SqlLess ThanOrEqualTo(object value)
    {
        return new SqlLess(SqlOperator.LessThanOrEqual, value);
    }

    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        if (!Parameters.Any())
        {
            return string.Empty;
        }

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} {_operator} {parameters.FirstOrDefault()?.ParameterName}";
    }
}
