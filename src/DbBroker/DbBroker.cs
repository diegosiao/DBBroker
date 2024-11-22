using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using DbBroker.Common.Model.Interfaces;
using DbBroker.Extensions;
using DbBroker.Model;

namespace DbBroker;

public static class DbBroker
{
    /// <summary>
    /// Key: Fully qualified name of the type implementing ISqlInsertTemplate />
    /// </summary>
    internal static Dictionary<string, ISqlInsertTemplate> SqlInsertTemplates = [];

    private static void ValidateDataModelContext<TDataModel>(DbConnection connection, TDataModel dataModel)
    {
        // 
    }

    public static bool Insert<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        ValidateDataModelContext(connection, dataModel);

        var insertColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (dataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
            .Select(x => x.Value);

        var parameters = insertColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(x.ColumnName, x.PropertyInfo.GetValue(dataModel)));

        var sqlInsert = dataModel.DataModelMap.SqlInsertTemplate.SqlTemplate
            .Replace("$$TABLEFULLNAME$$", dataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", string.Join(",", insertColumns.Select(x => x.ColumnName)))
            .Replace("$$PARAMETERS$$", string.Join(",", parameters.Select(x => x.ParameterName)));

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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <param name="connection"></param>
    /// <param name="include">
    ///     <para>The properties to be included in the SELECT command.</para>
    ///     <para>Regardless of the depth being loaded, if no property of the Data Model is specified, all properties will be included.</para>
    ///     <para>Collections are not loaded by default, it is necessary to explicitly include the collection property. Only collections on Data Model root properties are considered.</para>
    /// </param>
    /// <param name="orderByAsc"></param>
    /// <param name="orderByDesc"></param>
    /// <param name="transaction"></param>
    /// <param name="depth">The loading level for references. Default is zero, that means only the root value based properties are loaded.</param>
    public static SqlSelectCommand<TDataModel> Select<TDataModel>(
        this DbConnection connection, 
        IEnumerable<Expression<Func<TDataModel, object>>> include = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByAsc = null,
        IEnumerable<Expression<Func<TDataModel, object>>> orderByDesc = null,
        DbTransaction transaction = null,
        int depth = 0) where TDataModel : DataModel<TDataModel>
    {
        return new SqlSelectCommand<TDataModel>(Activator.CreateInstance<TDataModel>(), connection, include, orderByAsc, orderByDesc, transaction, depth);
    }

    /// <summary>
    /// CAUTION: The Key property is always ignored, no matter if it is in pristine state or not.
    /// <para>Call <see cref="SqlUpdateCommand{TDataModel}.AddFilter{TProperty}(System.Linq.Expressions.Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify explicitly the record to be updated.</para>
    /// 
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <param name="connection"></param>
    /// <param name="entity"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static SqlUpdateCommand<TDataModel> Update<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        var updateColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey)
            .Select(x => x.Value);

        var parameters = updateColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(x.ColumnName, x.PropertyInfo.GetValue(dataModel)));        

        return new SqlUpdateCommand<TDataModel>(dataModel, updateColumns, parameters, connection, transaction);
    }

    /// <summary>
    /// CAUTION: The Key property is always ignored, no matter if it is in pristine state or not.
    /// <para>Call <see cref="SqlDeleteCommand{TDataModel}.AddFilter{TProperty}(System.Linq.Expressions.Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify explicitly the record to be updated.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The data model instance that represents the database record to be deleted</typeparam>
    /// <param name="connection"></param>
    /// <param name="dataModel"></param>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public static SqlDeleteCommand<TDataModel> Delete<TDataModel>(this DbConnection connection, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        return new SqlDeleteCommand<TDataModel>(
            Activator.CreateInstance<TDataModel>(), 
            columns: [], 
            parameters: [], 
            connection, 
            transaction);
    }
}
