using DbBroker.Extensions;
using DbBroker.Model;
using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DbBroker;

/// <summary>
/// SQL INSERT command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class SqlInsertCommand<TDataModel> where TDataModel : DataModel<TDataModel>
{
    private TDataModel DataModel { get; set; }

    private DbConnection Connection { get; set; }

    private DbTransaction Transaction { get; set; }

    internal SqlInsertCommand(
        TDataModel dataModel,
        DbConnection connection,
        DbTransaction transaction)
    {
        DataModel = dataModel;
        Connection = connection;
        Transaction = transaction;
    }

    /// <summary>
    /// Executes the SQL command asynchronously
    /// </summary>
    /// <param name="commandTimeout">The time in seconds to wait the execution</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<int> ExecuteAsync(int commandTimeout = 0)
    {
        return await Task.Run(() => Execute(commandTimeout));
    }

    /// <summary>
    /// Executes the INSERT command and returns the number of affected rows for consistency.
    /// </summary>
    /// <param name="commandTimeout"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public int Execute(int commandTimeout = 0)
    {
        try
        {
            var insertColumns = DataModel
                .DataModelMap
                .MappedProperties
                .Where(x => DataModel.IsNotPristine(x.Value.PropertyName) && !x.Value.IsKey || (DataModel.DataModelMap.SqlInsertTemplate.IncludeKeyColumn && x.Value.IsKey))
                .Select(x => x.Value);

            var parameters = insertColumns
                .Select(x => DataModel.DataModelMap.Provider.GetDbParameter(DataModel, x))
                .ToList();

            var sqlInsert = DataModel.DataModelMap.SqlInsertTemplate.SqlTemplate
                .Replace("$$TABLEFULLNAME$$", DataModel.DataModelMap.TableFullName)
                .Replace("$$COLUMNS$$", string.Join(",", insertColumns.Select(x => x.ColumnName)))
                .Replace("$$PARAMETERS$$", string.Join(",", parameters.Select(x => x.ParameterName)))
                // Not required
                .Replace(
                    "$$KEY_COLUMN$$",
                    DataModel
                        .DataModelMap
                        .MappedProperties
                        .FirstOrDefault(x => x.Value.IsKey).Value?.ColumnName);

            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }

            DbParameter keyParameter = null;
            if (DataModel.DataModelMap.SqlInsertTemplate.TryRetrieveKey)
            {
                keyParameter = DataModel.DataModelMap.Provider.GetDbParameter("pKey", DBNull.Value);

                // TODO Improve the key data type handling: determine based on the mapped property type
                keyParameter.DbType = DataModel.DataModelMap.Provider.GetDbType(DataModel.DataModelMap.KeyProperty);
                keyParameter.Direction = ParameterDirection.Output;
                parameters.Add(keyParameter);
            }

            var command = Connection.CreateCommand();
            command.CommandText = DataModel.DataModelMap.SqlInsertTemplate.ReplaceParameters(sqlInsert);
            command.Parameters.AddRange(parameters.ToArray());
            command.Transaction = Transaction;
            command.CommandTimeout = commandTimeout;

            Debug.WriteLine(command.CommandText);

            if (DataModel.DataModelMap.SqlInsertTemplate.TryRetrieveKey)
            {
                var rowsAffected = command.ExecuteNonQuery();
                DataModel
                    .DataModelMap
                    .MappedProperties
                    .FirstOrDefault(x => x.Value.IsKey).Value.PropertyInfo.SetValue(DataModel, keyParameter.Value);
                return rowsAffected;
            }

            return command.ExecuteNonQuery();
        }
        catch (TargetInvocationException ex)
        {
            // TODO Adopt this error handling on all other methods
            Debug.WriteLine(ex.Message);
            throw new ApplicationException(ex.InnerException?.Message ?? ex.Message, ex?.InnerException ?? ex);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            throw new ApplicationException(ex.Message, ex);
        }
    }
}
