using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace DbBroker.Model;

public class SqlEquals : SqlExpression
{
    private SqlEquals(object value) : base(SqlOperator.Equals)
    {
        Parameters = [value];
    }

    public static SqlEquals To(object value)
    {
        return new SqlEquals(value);
    }

    public override string RenderSql(string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        return $"{columnName} = {parameters.FirstOrDefault()?.ParameterName}";
    }
}
