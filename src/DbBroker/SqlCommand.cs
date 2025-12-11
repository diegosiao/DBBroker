using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
    /// <summary>
    /// List of filters to apply to the SQL command
    /// </summary>
    protected List<CommandFilter> Filters { get; set; } = [];

    private int paramIndex = 0;

    /// <summary>
    /// The DataModel instance
    /// </summary>
    protected readonly TDataModel DataModel;

    /// <summary>
    /// The list of columns to be used in the SQL command
    /// </summary>
    protected readonly IEnumerable<DataModelMapProperty> Columns;

    /// <summary>
    /// The list of parameters to be used in the SQL command
    /// </summary>
    protected readonly IEnumerable<DbParameter> Parameters;

    /// <summary>
    /// The database connection
    /// </summary>
    protected readonly DbConnection Connection;

    /// <summary>
    /// The database transaction
    /// </summary>
    protected readonly DbTransaction Transaction;

    /// <summary>
    /// The SQL template to be used in the command
    /// </summary>
    protected string SqlTemplate { get; set; }

    /// <summary>
    /// Indicates if the command requires at least one filter to be executed
    /// </summary>
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
    
    /// <summary>
    /// Add a filter to the SQL command
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    public SqlCommand<TDataModel, TReturn> AddFilter<TProperty>(
        Expression<Func<TDataModel, TProperty>> propertyLambda, 
        SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        var commandFilter = new CommandFilter
        {
            DataModelMapProperty = DataModel.DataModelMap.MappedProperties[propertyName],
            SqlExpression = sqlExpression,
            // ChainedExpressions = chainedExpressions,
            Index = Filters.Count,
            Parameters = sqlExpression
                .Parameters
                .Select(x => DataModel.DataModelMap.Provider.GetDbParameter($"{propertyName}{Filters.Count}_f{paramIndex++}", x))
                .ToArray()
        };

        Filters.Add(commandFilter);

        return this;
    }

    /// <summary>
    /// Group an additional filter with AND operator to the last added filter
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public SqlCommand<TDataModel, TReturn> And<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        var commandFilter = new CommandFilter
        {
            DataModelMapProperty = DataModel.DataModelMap.MappedProperties[propertyName],
            SqlExpression = sqlExpression,
            Index = -1,
            Operator = SqlOperator.AND,
            Parameters = sqlExpression
                .Parameters
                .Select(x => DataModel.DataModelMap.Provider.GetDbParameter($"{propertyName}{Filters.Count}_f{paramIndex++}", x))
                .ToArray()
        };

        var lastFilter = Filters.LastOrDefault();

        if (lastFilter == null)
        {
            throw new InvalidOperationException("Cannot apply AND operator without a previous filter. ");
        }

        lastFilter.GroupedExpressions.Add(commandFilter);

        return this;
    }

    /// <summary>
    /// Group an additional filter with OR operator to the last added filter
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="propertyLambda"></param>
    /// <param name="sqlExpression"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public SqlCommand<TDataModel, TReturn> Or<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        var commandFilter = new CommandFilter
        {
            DataModelMapProperty = DataModel.DataModelMap.MappedProperties[propertyName],
            SqlExpression = sqlExpression,
            Index = -1,
            Operator = SqlOperator.OR,
            Parameters = sqlExpression
                .Parameters
                .Select(x => DataModel.DataModelMap.Provider.GetDbParameter($"{propertyName}{Filters.Count}_f{paramIndex++}", x))
                .ToArray()
        };

        var lastFilter = Filters.LastOrDefault();

        if (lastFilter == null)
        {
            throw new InvalidOperationException("Cannot apply AND operator without a previous filter. ");
        }

        lastFilter.GroupedExpressions.Add(commandFilter);

        return this;
    }

    /// <summary>
    /// Renders the SQL command text
    /// </summary>
    internal protected virtual string RenderSqlCommand()
    {
        return SqlTemplate
            .Replace("$$COLUMNS$$", string.Join(",", Columns.Select(x => $"{x.ColumnName}={Parameters.First(p => p.ParameterName[1..].Equals(x.ColumnName)).ParameterName}")))
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$FILTERS$$", Filters.RenderWhereClause()); // TODO: Expose a configuration flag to avoid not filtered UPDATE or DELETE by default
    }

    private DbCommand PrepareCommand(int commandTimeout = 0)
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

        return command;
    }

    /// <summary>
    /// Executes the SQL command
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait the execution</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual TReturn Execute(int commandTimeout = 0)
    {
        var command = PrepareCommand(commandTimeout);
        return (TReturn)(object)command.ExecuteNonQuery();
    }

    /// <summary>
    /// Executes the SQL command asynchronously
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait the execution</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public virtual async Task<TReturn> ExecuteAsync(int commandTimeout = 0)
    {
        var command = PrepareCommand(commandTimeout);
        return (TReturn)(object) await command.ExecuteNonQueryAsync();
    }
}
