using DbBroker.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DbBroker;

/// <summary>
/// SQL Expression for IN operator
/// </summary>
public class SqlExpressionIn : SqlExpression
{
    private SqlExpressionIn() : base(SqlOperator.In)
    {
    }

    /// <summary>
    /// Specifies the IN expression
    /// </summary>
    /// <returns></returns>
    public static SqlExpressionIn In()
    {
        return new SqlExpressionIn();
    }

    /// <summary>
    /// Renders the SQL for this expression
    /// </summary>
    /// <param name="alias"></param>
    /// <param name="columnName"></param>
    /// <param name="parameters"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public override string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index)
    {
        throw new NotImplementedException();
    }
}
