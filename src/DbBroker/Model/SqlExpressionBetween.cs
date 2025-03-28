using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

public class SqlBetween : SqlExpression
{
    private SqlBetween(object value1, object value2) : base(SqlOperator.Between)
    {
        if (value1 is not null && value2 is not null)
        {
            Parameters = [value1, value2];
        }
    }

    // TODO include the ability to compare with another column

    /// <summary>
    /// Column specified BETWEEN <paramref name="value1"/> AND <paramref name="value2"/>
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static SqlBetween These(object value1, object value2)
    {
        return new SqlBetween(value1, value2);
    }

    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        if (!Parameters.Any())
        {
            return string.Empty;
        }

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} BETWEEN {parameters.First()?.ParameterName} AND {parameters.ElementAt(1)?.ParameterName}";
    }
}
