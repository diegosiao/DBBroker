using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DbBroker.Model;

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

    public static SqlGreater Than(object value)
    {
        return new SqlGreater(SqlOperator.GreaterThan, value);
    }

    public static SqlGreater ThanOrEqualTo(object value)
    {
        return new SqlGreater(SqlOperator.GreaterThanOrEqual, value);
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
