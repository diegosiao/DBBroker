using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using DbBroker.Extensions;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker;

public abstract class SqlCommand<TDataModel, TReturn> : IFilteredCommand<TDataModel, TReturn> where TDataModel : DataModel<TDataModel>
{
    protected List<CommandFilter> Filters { get; set; } = [];

    protected readonly TDataModel DataModel;

    protected readonly IEnumerable<DataModelMapProperty> Columns;

    protected readonly IEnumerable<DbParameter> Parameters;

    protected readonly DbConnection Connection;

    protected readonly DbTransaction Transaction;

    protected string SqlTemplate { get; set; }

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

    public IFilteredCommand<TDataModel, TReturn> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        int paramIndex = 0;
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

    protected virtual string RenderSqlCommand()
    {
        return SqlTemplate
            .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", string.Join(",", Columns.Select(x => $"{x.ColumnName}={Parameters.First(p => p.ParameterName[1..].Equals(x.ColumnName)).ParameterName}")))
            .Replace("$$FILTERS$$", Filters.RenderWhereClause()); // TODO: Expose a configuration flag to avoid destructive UPDATE by default
    }

    public virtual TReturn Execute()
    {
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
        // command.CommandTimeout

        return (TReturn)(object)command.ExecuteNonQuery();
    }
}
