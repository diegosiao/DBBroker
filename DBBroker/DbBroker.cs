using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DbBroker.Common.Model.Interfaces;
using DbBroker.Model;
using DBBroker.Extensions;
using DBBroker.Model;

namespace DbBroker;

public static class DbBroker
{
    /// <summary>
    /// Key: Fully qualified name of the type implementing ISqlInsertTemplate />
    /// </summary>
    internal static Dictionary<string, ISqlInsertTemplate> SqlInsertTemplates = [];

    public static bool DbInsert<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModelBase<TDataModel>
    {
        var insertColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (dataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
            .Select(x => x.Value);

        var parameters = insertColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter($"@{x.ColumnName}", x.PropertyInfo.GetValue(dataModel)));

        var sqlInsert = dataModel.DataModelMap.SqlInsertTemplate.SqlTemplate
            .Replace("$$TABLEFULLNAME$$", dataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", string.Join(",", insertColumns.Select(x => x.ColumnName)))
            .Replace("$$PARAMETERS$$", string.Join(",", insertColumns.Select(x => $"@{x.ColumnName}")));

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        var command = connection.CreateCommand();
        command.CommandText = sqlInsert;
        command.Parameters.AddRange(parameters.ToArray());
        command.Transaction = transaction;       
        
        if (!dataModel.DataModelMap.SqlInsertTemplate.TryRetrieveKey)
        {
            command.ExecuteNonQuery();
            return true;
        }

        return true;
    }

    public static async Task<bool> DbInsertAsync<TDataModel>(TDataModel dataModel, DbTransaction transaction) where TDataModel : DataModelBase<TDataModel>
    {

        return true;
    }

    /// <summary>
    /// CAUTION: The Key property is ignored if in pristine state, call AddFilter() to specify explicitly the record to be updated.
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <param name="connection"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static SqlUpdateCommand<TDataModel> DbUpdate<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModelBase<TDataModel>
    {
        var updateColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey)
            .Select(x => x.Value);

        var parameters = updateColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter($"@{x.ColumnName}", x.PropertyInfo.GetValue(dataModel)));        

        return new SqlUpdateCommand<TDataModel>(dataModel, updateColumns, parameters, connection, transaction);
    }
}
