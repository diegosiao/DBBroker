using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbBroker.Model;

namespace DbBroker;

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

    public static SqlLike To(string value, bool lowerAll = true)
    {
        return new SqlLike(value, lowerAll);
    }

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
