using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

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

    public static SqlEquals To(object value, bool lowerAll = true)
    {
        return new SqlEquals(value, lowerAll);
    }

    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        if (!Parameters.Any())
        {
            return string.Empty;
        }

        if (_lowerAll)
        {
            return $"AND lower({(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName}) = lower({parameters.FirstOrDefault()?.ParameterName})";
        }

        return $"AND {(string.IsNullOrEmpty(alias) ? string.Empty : $"{alias}.")}{columnName} = {parameters.FirstOrDefault()?.ParameterName}";
    }
}
