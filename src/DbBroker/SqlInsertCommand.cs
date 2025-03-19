using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DbBroker.Extensions;
using DbBroker.Model;

namespace DbBroker;

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
