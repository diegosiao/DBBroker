using System;
using System.Linq.Expressions;

namespace DbBroker.Model;

/// <summary>
/// Represents an ORDER BY clause in SQL.
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class SqlOrderBy<TDataModel>
{
    private SqlOrderBy() { }

    internal Expression<Func<TDataModel, object>> Expression { get; private set; }

    internal bool Asc { get; private set; }

    /// <summary>
    /// Creates an ascending order by clause for the specified property.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static SqlOrderBy<TDataModel> Ascending(Expression<Func<TDataModel, object>> property)
    {
        return new SqlOrderBy<TDataModel>()
        {
            Expression = property,
            Asc = true
        };
    }

    /// <summary>
    /// Creates a descending order by clause for the specified property.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public static SqlOrderBy<TDataModel> Descending(Expression<Func<TDataModel, object>> property)
    {
        return new SqlOrderBy<TDataModel>()
        {
            Expression = property,
            Asc = false
        };
    }
}
