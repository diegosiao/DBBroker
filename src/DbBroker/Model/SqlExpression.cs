using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

public abstract class SqlExpression : ISqlExpression
{
    public SqlOperator SqlOperator { get; private set; }

    public IEnumerable<object> Parameters { get; protected set; } = [];

    public SqlExpression(SqlOperator sqlOperator)
    {
        SqlOperator = sqlOperator;
    }

    public abstract string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index);
}
