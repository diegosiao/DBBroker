using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

/// <summary>
/// Base class for all SQL Expressions
/// </summary>
public abstract class SqlExpression : ISqlExpression
{
    /// <summary>
    /// The SQL Operator for this expression
    /// </summary>
    public SqlOperator SqlOperator { get; private set; }

    /// <summary>
    /// The parameters for this expression
    /// </summary>
    public IEnumerable<object> Parameters { get; protected set; } = [];

    /// <summary>
    /// Constructor for SqlExpression
    /// </summary>
    /// <param name="sqlOperator"></param>
    public SqlExpression(SqlOperator sqlOperator)
    {
        SqlOperator = sqlOperator;
    }

    /// <summary>
    /// Renders the SQL for this expression
    /// </summary>
    /// <param name="alias"></param>
    /// <param name="columnName"></param>
    /// <param name="parameters"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public abstract string RenderSql(string alias, string columnName, IEnumerable<DbParameter> parameters, int index);
}
