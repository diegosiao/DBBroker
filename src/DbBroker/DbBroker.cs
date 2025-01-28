using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using DbBroker.Extensions;
using DbBroker.Model;

namespace DbBroker;

public static class DbBroker
{
    private static void ValidateDataModelContext<TDataModel>(DbConnection connection, TDataModel dataModel)
    {
        // TODO Validate if the 
    }

    /// <summary>
    /// Inserts a database record for the specified <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type to insert the record for.</typeparam>
    /// <param name="connection">The database connection to execute the SQL INSERT command.</param>
    /// <param name="dataModel">The <typeparamref name="TDataModel"/> instance to be inserted.</param>
    /// <param name="transaction">The database transaction to use to execute the SQL INSERT command.</param>
    /// <returns>Returns true if the insertion was executed without errors, false otherwise. </returns>
    public static bool Insert<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        ValidateDataModelContext(connection, dataModel);

        var insertColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (dataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
            .Select(x => x.Value);

        var parameters = insertColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(x.ColumnName, x.PropertyInfo.GetValue(dataModel)))
            .ToList();

        var sqlInsert = dataModel.DataModelMap.SqlInsertTemplate.SqlTemplate
            .Replace("$$TABLEFULLNAME$$", dataModel.DataModelMap.TableFullName)
            .Replace("$$COLUMNS$$", string.Join(",", insertColumns.Select(x => x.ColumnName)))
            .Replace("$$PARAMETERS$$", string.Join(",", parameters.Select(x => x.ParameterName)))
            // Not required
            .Replace("$$KEY_COLUMN$$", dataModel
            .DataModelMap
            .MappedProperties
            .FirstOrDefault(x => x.Value.IsKey).Value?.ColumnName);

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        DbParameter keyParameter = null;
        if (dataModel.DataModelMap.SqlInsertTemplate.TryRetrieveKey)
        {
            keyParameter = dataModel.DataModelMap.Provider.GetDbParameter("pKey", DBNull.Value);

            // TODO Improve the key data type handling: determine based on the mapped property type
            keyParameter.DbType = dataModel.DataModelMap.Provider.GetDbType(dataModel.DataModelMap.KeyProperty);
            keyParameter.Direction = ParameterDirection.Output;
            parameters.Add(keyParameter);
        }

        var command = connection.CreateCommand();
        command.CommandText = dataModel.DataModelMap.SqlInsertTemplate.ReplaceParameters(sqlInsert);
        command.Parameters.AddRange(parameters.ToArray());
        command.Transaction = transaction;

        if (dataModel.DataModelMap.SqlInsertTemplate.TryRetrieveKey)
        {
            command.ExecuteNonQuery();
            dataModel
                .DataModelMap
                .MappedProperties
                .FirstOrDefault(x => x.Value.IsKey).Value.PropertyInfo.SetValue(dataModel, keyParameter.Value);
            return true;
        }

        command.ExecuteNonQuery();
        return true;
    }

    /// <summary>
    /// Inserts or updates a database record for the specified <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type to insert the record for.</typeparam>
    /// <param name="connection">The database connection to execute the SQL MERGE command.</param>
    /// <param name="dataModel">The <typeparamref name="TDataModel"/> instance to be inserted.</param>
    /// <param name="transaction">The database transaction to use to execute the SQL MERGE command.</param>
    /// <returns>Returns true if the command was executed without errors, false otherwise. </returns>
    public static bool Upsert<TDataModel>(this DbConnection connection, TDataModel dataModel, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        var upsertColumns = dataModel
            .DataModelMap
            .MappedProperties
            .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (dataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
            .Select(x => x.Value);

        var parameters = upsertColumns
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(x.ColumnName, x.PropertyInfo.GetValue(dataModel)))
            .ToList();

        var sqlUpsertCommand = new SqlUpsertCommand<TDataModel>(dataModel, upsertColumns, parameters, connection, transaction);

        sqlUpsertCommand.Execute();
        return true;
    }

    /// <summary>
    /// Retrieves database record(s) for <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <param name="connection"></param>
    /// <param name="load">
    ///     <para>Properties to be included in the SQL SELECT command as columns. Keys are implicitly included.</para>
    ///     <para>PROPERTIES: Regardless of the depth being loaded, if no property of the Data Model is specified all properties will be included.</para>
    ///     <para>COLLECTIONS: Not loaded by default, it is necessary to explicitly include collection properties. Only collections on Data Model root properties can be included. Only root properties of collections objects are loaded, regardless of the depth specified.</para>
    /// </param>
    /// <param name="ignore">Properties to be ignored when building the SQL SELECT command. Have precedence regarding <paramref name="load"/> parameter.</param>
    /// <param name="orderBy"></param>
    /// <param name="transaction">The database transaction to use to execute this command.</param>
    /// <param name="depth">The loading level for references. Default is zero, that means only the root value based properties from the Data Model are loaded. Needs to be coherent with <paramref name="load"/>.</param>
    /// <param name="skip">Number of records to skip at the begining of the result. Be careful when including collections as you might get truncated results.</param>
    /// <param name="take">Number of records to take from the result. Be careful when including collections as you might get truncated results.</param>
    public static SqlSelectCommand<TDataModel> Select<TDataModel>(
        this DbConnection connection,
        IEnumerable<Expression<Func<TDataModel, object>>> load = null,
        IEnumerable<Expression<Func<TDataModel, object>>> ignore = null,
        DbTransaction transaction = null,
        int depth = 0,
        int? skip = null,
        int? take = null) where TDataModel : DataModel<TDataModel>
    {
        return new SqlSelectCommand<TDataModel>(Activator.CreateInstance<TDataModel>(), connection, load, ignore, transaction, depth, skip ?? 0, take ?? 0);
    }

    /// <summary>
    /// CAUTION: The Key property is always ignored, no matter if it is in pristine state or not.
    /// <para>Call <see cref="SqlUpdateCommand{TDataModel}.AddFilter{TProperty}(Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify explicitly the record to be updated.</para>
    /// </summary>
    /// <typeparam name="TDataModel"></typeparam>
    /// <param name="connection"></param>
    /// <param name="dataModel"></param>
    /// <param name="transaction">The database transaction to use to execute this command.</param>
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
    /// <para>Deletes database record(s).</para>
    /// CAUTION: Chain a call to <see cref="SqlCommand{TDataModel, TReturn}.AddFilter{TProperty}(Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify the record(s) to be deleted.
    /// </summary>
    /// <typeparam name="TDataModel">The data model instance that represents the database record to be deleted</typeparam>
    /// <param name="connection"></param>
    /// <param name="dataModel"></param>
    /// <param name="transaction">The database transaction to use to execute this command.</param>
    /// <returns></returns>
    public static SqlDeleteCommand<TDataModel> Delete<TDataModel>(this DbConnection connection, DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        return new SqlDeleteCommand<TDataModel>(
            Activator.CreateInstance<TDataModel>(),
            connection,
            transaction);
    }
}
