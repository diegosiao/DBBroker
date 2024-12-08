using System;
using System.Linq.Expressions;

namespace DbBroker.Model;

public class SqlOrderBy<TDataModel>
{
    private SqlOrderBy() { }

    internal Expression<Func<TDataModel, object>> Expression {get; private set;}

    internal bool Asc {get; private set;}

    public static SqlOrderBy<TDataModel> Ascending(Expression<Func<TDataModel, object>> property)
    {
        return new SqlOrderBy<TDataModel>(){
            Expression = property,
            Asc = true
        };
    }

    public static SqlOrderBy<TDataModel> Descending(Expression<Func<TDataModel, object>> property)
    {
        return new SqlOrderBy<TDataModel>()
        {
            Expression = property,
            Asc = false
        };
    }
}
