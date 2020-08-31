using DBBroker.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace DBBroker.Engine
{
    /// <summary>
    /// <para>This class executes any SQL command through the static method <see cref="DBBrokerLive"/>.ExecCmdSQL() which transforms rows of the first result set returning an instance of <see cref="DBBrokerLiveRowCollection"/>.</para>
    /// <para>This approach is great only because of the runtime resolution and flexibility, but in the other hand is error prone. Always prefer the conventional mapping for data access.</para>
    /// </summary>
    public class DBBrokerLive
    {
        private DBBrokerLive() { }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: null, context: null, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, context: null, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        /// </param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter, Configuration context)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, context: context, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter, Configuration context, CommandType commandType)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, context: null, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter, Configuration context, CommandType commandType, DbTransaction transaction)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, context: null, commandType: CommandType.Text, transaction: transaction, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back.</param>
        /// <param name="entityName">An alias to the entity formed by the result of this execution.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter, Configuration context, CommandType commandType, DbTransaction transaction, string entityName)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, context: null, commandType: CommandType.Text, transaction: transaction, entityName: entityName);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, List<DbParameter> parameters)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, context: null, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, List<DbParameter> parameters, Configuration context)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, context: context, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, List<DbParameter> parameters, Configuration context, CommandType commandType)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, context: null, commandType: CommandType.Text, transaction: null, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, List<DbParameter> parameters, Configuration context, CommandType commandType, DbTransaction transaction)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, context: null, commandType: CommandType.Text, transaction: transaction, entityName: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back.</param>
        /// <param name="entityName">An alias to the entity formed by the result of this execution.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, List<DbParameter> parameters, Configuration context, CommandType commandType, DbTransaction transaction, string entityName)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, context: null, commandType: CommandType.Text, transaction: transaction, entityName: entityName);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="DBBrokerLiveRowCollection"/> with the data from rows as instances of <see cref="DBBrokerLiveRow"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        ///     <para>As a convenience to avoid the creation of a DbParameterCollection instance when only one parameter will be used.</para>
        ///     <para>If informed, the value of '<paramref name="parameters"/>' will be ignored.</para>
        /// </param>
        /// <param name="parameters">Parameters used in the specified SQL command or script.</param>
        /// <param name="context">
        ///     <para>The configuration context for this execution defined by a line in DBBroker.config file. You can use <see cref="Configuration.GetByType{T}"/> to get one.</para>
        ///     <para>If not informed, the first line of DBBroker.config file will be assumed.</para>
        /// </param>
        /// <param name="commandType">Informs the type of the SQL specified.</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back.</param>
        /// <param name="entityName">An alias to the entity formed by the result of this execution.</param>
        public static DBBrokerLiveRowCollection ExecCmdSQL(string cmdText, DbParameter parameter = null, List<DbParameter> parameters = null, Configuration context = null, CommandType commandType = CommandType.Text, DbTransaction transaction = null, string entityName = "")
        {
            DBBroker<object>.RecordsAffected = 0;

            if (string.IsNullOrEmpty(entityName))
                entityName = Resources.Unnamed;

            if (string.IsNullOrEmpty(cmdText))
                cmdText = "SELECT ''; ";

            var objects = new DBBrokerLiveRowCollection();

            var isExternalTransaction = transaction != null;

            if (isExternalTransaction && transaction.Connection == null)
                throw new DBBrokerException(Resources.ErrorTranAssociatedConnectionNull);

            DbConnection connection;

            if (isExternalTransaction)
                connection = transaction.Connection;
            else
                connection = Configuration.GetOpenConnection(context);

            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = cmdText;
                    command.CommandType = commandType;

                    if (transaction != null)
                        command.Transaction = transaction;

                    if (parameter != null)
                        command.Parameters.Add(parameter);

                    if (parameters != null && parameters.Count > 0)
                        foreach (DbParameter param in parameters)
                            command.Parameters.Add(param);

                    using (var reader = command.ExecuteReader())
                    {
                        DBBroker<object>.RecordsAffected = reader.RecordsAffected;

                        if (reader.HasRows)
                        {
                            string[] fields = new string[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                                fields[i] = reader.GetName(i);

                            while (reader.Read())
                            {
                                var obj = new DBBrokerLiveRow(fields, entityName);

                                for (int i = 0; i < fields.Length; i++)
                                    obj[i] = reader.GetValue(i);

                                objects.Add(obj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Debugger.IsAttached)
                    throw new DBBrokerException(string.Format(Resources.ErrorExecutingSqlCommand, new string[] { ex.Message, cmdText }), ex);
                else
                    throw new DBBrokerException(Resources.ErrorMessageDefault, ex);
            } 
            finally
            {
                try
                {
                    if (!isExternalTransaction)
                        connection?.Dispose();
                }
                catch { /* Any errors trying to dispose the connection can be safely omitted */ }
            }

            return objects;
        }

        /// <summary>
        /// Gets an instance of a initialized database transaction with its own open connection.
        /// </summary>
        /// <param name="context">
        ///     <para>The configuration context specified in DBBroker.config file from which this database transaction should be generated.</para>
        ///     <para>You can get one using <see cref="Configuration.GetByType{T}"/>. If null, the first line of the DBBroker.config file is assumed.</para>
        /// </param>
        public static DbTransaction GetTransaction(Configuration context)
        {
            try
            {
                DbTransaction tran = Configuration.GetOpenConnection(context).BeginTransaction();
                return tran;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets an instance of a database transaction with its own open connection.
        /// </summary>
        /// <param name="context">
        ///     <para>The configuration context specified in DBBroker.config file from which this database transaction should be generated.</para>
        ///     <para>You can get one using <see cref="Configuration.GetByType{T}"/>. If null, the first line of the DBBroker.config file is assumed.</para>
        /// </param>
        /// <param name="isolationLevel">The database isolation level for this transaction</param>
        public static DbTransaction GetTransaction(Configuration context, IsolationLevel isolationLevel)
        {
            try
            {
                return Configuration.GetOpenConnection(context).BeginTransaction(isolationLevel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
