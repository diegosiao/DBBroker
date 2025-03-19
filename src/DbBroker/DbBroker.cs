using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DbBroker.Extensions;
using DbBroker.Model;

namespace DbBroker;

public static class DbBroker
{
    private const string AggregationPropertyNotInRootMessage = "The aggregation property needs to be at the root of the Data Model specified";

    /// <summary>
    /// Inserts a database record for the specified <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database record to be inserted</typeparam>
    /// <param name="connection">The database connection to execute the SQL INSERT command.</param>
    /// <param name="dataModel">The <typeparamref name="TDataModel"/> instance to be inserted.</param>
    /// <param name="transaction">The database transaction to use to execute the SQL INSERT command.</param>
    /// <returns>Returns the number of rows affected.</returns>
    public static SqlInsertCommand<TDataModel> Insert<TDataModel>(
        this DbConnection connection, 
        TDataModel dataModel, 
        DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        return new SqlInsertCommand<TDataModel>(dataModel, connection, transaction);
    }

    /// <summary>
    /// Inserts or updates a database record for the specified <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database record to be inserted or updated</typeparam>
    /// <param name="connection">The database connection to execute the SQL MERGE command.</param>
    /// <param name="dataModel">The <typeparamref name="TDataModel"/> instance to be inserted.</param>
    /// <param name="transaction">The database transaction to use to execute the SQL MERGE command.</param>
    /// <returns>Number of rows affected</returns>
    public static SqlUpsertCommand<TDataModel> Upsert<TDataModel>(
        this DbConnection connection, 
        TDataModel dataModel, 
        DbTransaction transaction = null) where TDataModel : DataModel<TDataModel>
    {
        try
        {
            var upsertColumns = dataModel
                .DataModelMap
                .MappedProperties
                .Where(x => dataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (dataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
                .Select(x => x.Value);

            var parameters = upsertColumns
                .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(x.ColumnName, x.PropertyInfo.GetValue(dataModel)))
                .ToList();

            return new SqlUpsertCommand<TDataModel>(dataModel, upsertColumns, parameters, connection, transaction);
        }
        catch (TargetInvocationException ex)
        {
            Debug.WriteLine(ex.Message);
            throw new ApplicationException(ex.InnerException?.Message ?? ex.Message, ex?.InnerException ?? ex);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to upsert the record: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Retrieves database record(s) for <typeparamref name="TDataModel"/>.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database records to be selected</typeparam>
    /// <param name="connection"></param>
    /// <param name="load">
    ///     <para>Properties to be included in the SQL SELECT command as columns. Keys are implicitly included.</para>
    ///     <para>PROPERTIES: Regardless of the depth being loaded, if no property of the Data Model is specified all properties will be included.</para>
    ///     <para>COLLECTIONS: Not loaded by default, it is necessary to explicitly include collection properties. Only collections on Data Model root properties can be included. Only root properties of collections objects are loaded, regardless of the depth specified.</para>
    /// </param>
    /// <param name="ignore">Properties to be ignored when building the SQL SELECT command. Have precedence regarding <paramref name="load"/> parameter.</param>
    /// <param name="orderBy"></param>
    /// <param name="transaction">The database transaction to use to execute this command.</param>
    /// <param name="depth">The loading level for references. Default is zero, that means only the root value based properties from the Data Model are loaded. Needs to represent the same depth or deeper than properties specified by '<paramref name="load"/>' parameter.</param>
    /// <param name="skip">Number of records to skip at the begining of the result. Be careful when including collections as you might get truncated results.</param>
    /// <param name="take">Number of records to take from the result. Be careful when including collections as you might get truncated results.</param>
    public static SqlSelectCommand<TDataModel> Select<TDataModel>(
        this DbConnection connection,
        IEnumerable<Expression<Func<TDataModel, object>>> load = null,
        IEnumerable<Expression<Func<TDataModel, object>>> ignore = null,
        DbTransaction transaction = null,
        int depth = 0) where TDataModel : DataModel<TDataModel>
    {
        return new SqlSelectCommand<TDataModel>(Activator.CreateInstance<TDataModel>(), connection, load, ignore, transaction, depth);
    }

    /// <summary>
    /// CAUTION: The Key property is always ignored, no matter if it is in pristine state or not.
    /// <para>Call <see cref="SqlUpdateCommand{TDataModel}.AddFilter{TProperty}(Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify explicitly the record to be updated.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database record to be updated</typeparam>
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
            .Select(x => dataModel.DataModelMap.Provider.GetDbParameter(dataModel, dataModel.DataModelMap.MappedProperties[x.PropertyName]));

        return new SqlUpdateCommand<TDataModel>(
            dataModel, 
            updateColumns, 
            parameters, 
            connection, 
            transaction);
    }

    /// <summary>
    /// <para>Deletes database record(s).</para>
    /// CAUTION: Chain a call to <see cref="SqlCommand{TDataModel, TReturn}.AddFilter{TProperty}(Expression{Func{TDataModel, TProperty}}, SqlExpression)"/> to specify the record(s) to be deleted.
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database record to be deleted</typeparam>
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

    /// <summary>
    /// <para>Scalar aggregation count over the table represented by the <typeparamref name="TDataModel"/> specified.</para>
    /// <para>Why there is no more options to this aggregation? Database Aggregations create columns at execution time, DBBroker's philosophy is based on reflecting database structure state.</para>
    /// <para>If you need a grouped list with multiple counts, create a database View then run `dbbroker sync` to consume it through the Data Model generated.</para>
    /// </summary>
    /// <typeparam name="TDataModel">The Data Model type that represents the database table the SQL COUNT(*) function will run against.</typeparam>
    /// <param name="connection"></param>
    /// <param name="groupBy"></param>
    /// <param name="dbTransaction"></param>
    /// <returns></returns>
    public static SqlSelectCountCommand<TDataModel> Count<TDataModel>(this DbConnection connection, DbTransaction dbTransaction = null) where TDataModel : DataModel<TDataModel>
    {
        return new SqlSelectCountCommand<TDataModel>(
            Activator.CreateInstance<TDataModel>(),
            connection,
            dbTransaction);
    }

    public static SqlSelectSumCommand<TDataModel, TResult> Sum<TDataModel, TResult>(this DbConnection connection, Expression<Func<TDataModel, object>> property, DbTransaction dbTransaction = null) where TDataModel : DataModel<TDataModel>
    {
        if (!PropertyPathHelper.IsRootProperty(property))
        {
            throw new ArgumentException($"{AggregationPropertyNotInRootMessage} ({typeof(TDataModel).Name}).", nameof(property));
        }

        var dataModelInstance = Activator.CreateInstance<TDataModel>();
        return new SqlSelectSumCommand<TDataModel, TResult>(
            dataModelInstance,
            dataModelInstance.DataModelMap.MappedProperties[PropertyPathHelper.GetNestedPropertyPath(property)],
            connection,
            dbTransaction);
    }

    public static SqlSelectAvgCommand<TDataModel, TResult> Avg<TDataModel, TResult>(this DbConnection connection, Expression<Func<TDataModel, object>> property, DbTransaction dbTransaction = null) where TDataModel : DataModel<TDataModel>
    {
        if (!PropertyPathHelper.IsRootProperty(property))
        {
            throw new ArgumentException($"{AggregationPropertyNotInRootMessage} ({typeof(TDataModel).Name}).", nameof(property));
        }

        var dataModelInstance = Activator.CreateInstance<TDataModel>();
        return new SqlSelectAvgCommand<TDataModel, TResult>(
            dataModelInstance,
            dataModelInstance.DataModelMap.MappedProperties[PropertyPathHelper.GetNestedPropertyPath(property)],
            connection,
            dbTransaction);
    }


    public static SqlSelectMinCommand<TDataModel, TResult> Min<TDataModel, TResult>(this DbConnection connection, Expression<Func<TDataModel, object>> property, DbTransaction dbTransaction = null) where TDataModel : DataModel<TDataModel>
    {
        if (!PropertyPathHelper.IsRootProperty(property))
        {
            throw new ArgumentException($"{AggregationPropertyNotInRootMessage} ({typeof(TDataModel).Name}).", nameof(property));
        }

        var dataModelInstance = Activator.CreateInstance<TDataModel>();
        return new SqlSelectMinCommand<TDataModel, TResult>(
            dataModelInstance,
            dataModelInstance.DataModelMap.MappedProperties[PropertyPathHelper.GetNestedPropertyPath(property)],
            connection,
            dbTransaction);
    }


    public static SqlSelectMaxCommand<TDataModel, TResult> Max<TDataModel, TResult>(this DbConnection connection, Expression<Func<TDataModel, object>> property, DbTransaction dbTransaction = null) where TDataModel : DataModel<TDataModel>
    {
        if (!PropertyPathHelper.IsRootProperty(property))
        {
            throw new ArgumentException($"{AggregationPropertyNotInRootMessage} ({typeof(TDataModel).Name}).", nameof(property));
        }

        var dataModelInstance = Activator.CreateInstance<TDataModel>();
        return new SqlSelectMaxCommand<TDataModel, TResult>(
            dataModelInstance,
            dataModelInstance.DataModelMap.MappedProperties[PropertyPathHelper.GetNestedPropertyPath(property)],
            connection,
            dbTransaction);
    }
}
