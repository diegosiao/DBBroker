using System.Collections.Generic;
using System.Data.Common;

namespace DbBroker.Model.Interfaces;

internal interface ISqlExpression
{
    SqlOperator SqlOperator { get; }

    IEnumerable<object> Parameters { get; }

    string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index);
}
