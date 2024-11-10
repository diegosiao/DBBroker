using System.Collections.Generic;
using System.Data.Common;

namespace DbBroker.Model.Interfaces;

public interface ISqlExpression
{
    SqlOperator SqlOperator { get; }

    IEnumerable<object> Parameters { get; }

    string RenderSql(string columnName, int index);
}
