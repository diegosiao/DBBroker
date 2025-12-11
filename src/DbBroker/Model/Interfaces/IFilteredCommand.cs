using System;
using System.Linq.Expressions;

namespace DbBroker.Model.Interfaces;

/// <summary>
/// Interface for commands that can have filters applied to them
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public interface IFilteredCommand<TDataModel, TReturn> where TDataModel : DataModel<TDataModel>
{
    /// <summary>
    /// Add a filter to the command
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    SqlCommand<TDataModel, TReturn> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression);

    /// <summary>
    /// Group an AND expression to the last added filter
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    SqlCommand<TDataModel, TReturn> And<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression);

    /// <summary>
    /// Group an OR expression to the last added filter
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    SqlCommand<TDataModel, TReturn> Or<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression);

    /// <summary>
    /// Execute the command with the applied filters
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    TReturn Execute(int commandTimeout = 0);
}
