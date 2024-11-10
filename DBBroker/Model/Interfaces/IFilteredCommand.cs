using System;
using System.Linq.Expressions;
using DBBroker.Model;

namespace DbBroker.Model.Interfaces;

public interface IFilteredCommand<TDataModel> where TDataModel : DataModelBase<TDataModel>
{
    IFilteredCommand<TDataModel> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression);

    int Execute();
}
