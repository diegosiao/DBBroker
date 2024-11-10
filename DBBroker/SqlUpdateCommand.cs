using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using DbBroker.Extensions;
using DbBroker.Model;
using DbBroker.Model.Interfaces;
using DBBroker.Extensions;
using DBBroker.Model;

namespace DbBroker;

public class SqlUpdateCommand<TDataModel> : IFilteredCommand<TDataModel> where TDataModel : DataModelBase<TDataModel>
{
    private List<CommandFilter> Filters { get; set; } = [];

    private readonly TDataModel _dataModel;

    private readonly IEnumerable<DataModelMapProperty> _updateColumns;

    private readonly IEnumerable<DbParameter> _updateParameters;

    private readonly DbConnection _connection;

    private readonly DbTransaction _transaction;

    internal SqlUpdateCommand(
        TDataModel dataModel,
        IEnumerable<DataModelMapProperty> updateColumns,
        IEnumerable<DbParameter> updateParameters,
        DbConnection connection,
        DbTransaction transaction)
    {
        _dataModel = dataModel;
        _updateColumns = updateColumns;
        _updateParameters = updateParameters;
        _connection = connection;
        _transaction = transaction;
    }

    public IFilteredCommand<TDataModel> AddFilter<TProperty>(Expression<Func<TDataModel, TProperty>> propertyLambda, SqlExpression sqlExpression)
    {
        var propertyName = ((MemberExpression)propertyLambda.Body).Member.Name;

        Filters.Add(new CommandFilter{
            DataModelMapProperty = _dataModel.DataModelMap.MappedProperties[propertyName],
            SqlExpression = sqlExpression,
            Index = Filters.Count
        });
        
        return this;
    }

    public int Execute()
    {
        var sqlUpdate = Constants.SqlUpdateTemplate
            .Replace("$$TABLEFULLNAME$$", _dataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", string.Join(",", _updateColumns.Select(x => $"{x.ColumnName}=@{x.ColumnName}")))
            .Replace("$$FILTERS$$", Filters.Any() ? Filters.RenderWhereClause() : "1=1"); // TODO: Expose a configuration flag to avoid destructive UPDATE by default

        if (_connection.State != ConnectionState.Open)
        {
            _connection.Open();
        }

        List<DbParameter> filterParameters = [];
        foreach (var filter in Filters)
        {
            filterParameters.AddRange(filter
                .SqlExpression
                .Parameters
                .Select(x => _dataModel.DataModelMap.Provider.GetDbParameter($"@{filter.DataModelMapProperty.ColumnName}{filter.Index}", x)));
        }

        var command = _connection.CreateCommand();
        command.CommandText = sqlUpdate;
        command.Parameters.AddRange(_updateParameters.ToArray());
        command.Parameters.AddRange(filterParameters.ToArray());
        command.Transaction = _transaction;

        return command.ExecuteNonQuery();
    }
}
