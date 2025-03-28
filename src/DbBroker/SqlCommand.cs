using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DbBroker.Extensions;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker;

/// <summary>
/// Base class for all SQL Commands
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
/// <typeparam name="TReturn"></typeparam>
public abstract class SqlCommand<TDataModel, TReturn> : IFilteredCommand<TDataModel, TReturn> where TDataModel : DataModel<TDataModel>
{
    protected List<CommandFilter> Filters { get; set; } = [];

    protected readonly TDataModel DataModel;

    protected readonly IEnumerable<DataModelMapProperty> Columns;

    protected readonly IEnumerable<DbParameter> Parameters;

    protected readonly DbConnection Connection;

    protected readonly DbTransaction Transaction;

    protected string SqlTemplate { get; set; }

    protected bool RequireFilter { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataModel"></param>
    /// <param name="columns"></param>
    /// <param name="parameters"></param>
    /// <param name="connection"></param>
    /// <param name="transaction"></param>
    /// <param name="sqlTemplate"></param>
    public SqlCommand(TDataModel dataModel,
        IEnumerable<DataModelMapProperty> columns,
        IEnumerable<DbParameter> parameters,
        DbConnection connection,
        DbTransaction transaction,
        string sqlTemplate)
    {
        DataModel = dataModel;
        Columns = columns;
        Parameters = parameters;
        Connection = connection;
        Transaction = transaction;
        SqlTemplate = sqlTemplate;
    }

    int paramIndex = 0;

    /// <summary>
    /// Add a filter to the SQL command
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    public SqlCommand<TDataModel, TReturn> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        Filters.Add(new CommandFilter
        {
            DataModelMapProperty = DataModel.DataModelMap.MappedProperties[propertyName],
            SqlExpression = sqlExpression,
            Index = Filters.Count,
            Parameters = sqlExpression
                .Parameters
                .Select(x => DataModel.DataModelMap.Provider.GetDbParameter($"{propertyName}{Filters.Count}_f{paramIndex++}", x))
                .ToArray()
        });

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    internal protected virtual string RenderSqlCommand()
    {
        return SqlTemplate
            .Replace("$$COLUMNS$$", string.Join(",", Columns.Select(x => $"{x.ColumnName}={Parameters.First(p => p.ParameterName[1..].Equals(x.ColumnName)).ParameterName}")))
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$FILTERS$$", Filters.RenderWhereClause()); // TODO: Expose a configuration flag to avoid not filtered UPDATE or DELETE by default
    }

    /// <summary>
    /// Executes the SQL command
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait the execution</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual TReturn Execute(int commandTimeout = 0)
    {
        if (RequireFilter && Filters.Count == 0)
        {
            throw new InvalidOperationException("This operation is potentially destructive and requires a filter. ");
        }

        if (Connection.State != ConnectionState.Open)
        {
            Connection.Open();
        }

        List<DbParameter> filterParameters = [];
        foreach (var filter in Filters)
        {
            filterParameters.AddRange(filter.Parameters);
        }

        var command = Connection.CreateCommand();
        command.CommandText = RenderSqlCommand();
        command.Parameters.AddRange(Parameters.ToArray());
        command.Parameters.AddRange(filterParameters.ToArray());
        command.Transaction = Transaction;
        command.CommandTimeout = commandTimeout;

        Debug.WriteLine(command.CommandText);

        return (TReturn)(object)command.ExecuteNonQuery();
    }
}
