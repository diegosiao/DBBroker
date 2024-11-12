using System;
using System.Linq.Expressions;
using DbBroker.Model;

namespace DbBroker.Model.Interfaces;

public interface IFilteredCommand<TDataModel, TReturn> where TDataModel : DataModel<TDataModel>
{
    IFilteredCommand<TDataModel, TReturn> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression);

    TReturn Execute();
}
