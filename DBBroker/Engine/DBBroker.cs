using System;
using System.Collections.Generic;
using DBBroker.Mapping;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using DBBroker.Properties;
using System.Collections;

namespace DBBroker.Engine
{
    //compile with: /doc:DBBroker.xml  

    /// <summary>
    /// <para>This is the special class that your data access classes should inherit from.</para>
    /// <para>Example:</para>
    /// public class DBPerson : DBBroker&lt;Person>
    /// <para>{</para>
    /// <para>}</para>
    /// <para>To contribute or learn visit http://www.getdbbroker.com</para>
    /// </summary>
    /// <typeparam name="T">The context type for database interactions.</typeparam>
    public class DBBroker<T>
    {
        /// <summary>
        /// Informs if the properties of type <see cref="List{T}"/> should not be loaded.
        /// </summary>
        public static bool IgnoreLists { get; set; }

        /// <summary>
        /// Depending on the session configuration, shows the number of affected records in the last execution
        /// </summary>
        public static int RecordsAffected { get; set; }

        /// <summary>
        /// Gets an instance of a database transaction with its own open connection.
        /// </summary>
        public static DbTransaction GetTransaction()
        {
            try
            {
                DbTransaction tran = Configuration.GetOpenConnection<T>().BeginTransaction();
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
        /// <param name="isolationLevel">The database isolation level for this transaction</param>
        public static DbTransaction GetTransaction(IsolationLevel isolationLevel)
        {
            try
            {
                DbTransaction tran = Configuration.GetOpenConnection<T>().BeginTransaction(isolationLevel);
                return tran;
            }
            catch (Exception ex)
            {
                throw ex; // new Exception(string.Format(Resources.ErrorUnableToConnect, ex.Message), ex);
            }
        }

        /// <summary>
        /// Inserts or updates the database record of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The instance to be persisted</param>
        public static void Save(T obj)
        {
            Save(obj, null);
        }

        /// <summary>
        /// Inserts or updates the database record of <typeparamref name="T"/> using a database transaction.
        /// </summary>
        /// <param name="obj">The instance to be persisted</param>
        /// <param name="transaction">
        ///     <para>The database transaction in which this execution should run.</para>
        ///     <para>WARNING: If specified, make sure to commit appropriately.</para>
        /// </param>
        public static void Save(T obj, DbTransaction transaction)
        {
            if (obj == null)
                return;

            bool isExternalTran = true;

            if (transaction == null)
            {
                transaction = GetTransaction();
                isExternalTran = false;
            }

            try
            {
                var databaseContext = Configuration.GetByType<T>();
                long id_value = long.Parse(GetIdValue(obj).ToString());

                //TODO: What to do when an Oracle operation must insert the id?
                //if(databaseContext.DatabaseContext == SupportedDatabases.Oracle)
                bool update = (id_value > 0);

                if (update)
                {
                    List<DbParameter> parametros = new List<DbParameter>();
                    LoadParameters(obj, parametros);

                    DBBrokerLive.ExecCmdSQL(GetUpdateStmt(obj), 
                        parameters: parametros, 
                        commandType: CommandType.Text, 
                        transaction: transaction,
                        context: databaseContext);
                }
                else
                {
                    List<DbParameter> parametros = new List<DbParameter>();
                    LoadParameters(obj, parametros);

                    //DBMappedClass map = null;
                    //if (SupportedDatabases.Oracle == databaseContext.DatabaseContext && !string.IsNullOrEmpty((map = GetMapeamento(obj)).Sequence))
                    //    parametros.Add(Configuration.GetParameter<T>(map.PrimaryKey, GetIdValue(obj)));
                    
                    DBBrokerLive.ExecCmdSQL(
                        GetInsertStmt(obj),
                        parameters: parametros,
                        commandType: CommandType.Text,
                        transaction: transaction,
                        context: databaseContext);

                    var result = DBBrokerLive.ExecCmdSQL(
                        Configuration.GetLastInsertedIdFunction<T>(GetMapeamento(obj)),
                        commandType: CommandType.Text,
                        transaction: transaction,
                        context: databaseContext);

                    long new_id = 0;
                    if (result.Count > 0
                        && result[0].ColumnNames.Count > 0
                        && long.TryParse(result[0][0].ToString(), out new_id))
                        SetIdValue(obj, result[0][0]);
                }

                if (!IgnoreLists)
                    SaveMappedListsReferences(obj, transaction);

                if (!isExternalTran)
                {
                    transaction.Commit();
                    transaction.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (transaction != null && transaction.Connection != null)
                    try { transaction.Rollback(); }
                    catch(InvalidOperationException) { /* have already been rolledback */ }

                if (Debugger.IsAttached)
                    throw ex;
                else
                    throw new DBBrokerException(string.Format(Resources.ErrorSavingObject, obj.GetType().Name), ex);
            }
        }

        private static void SaveMappedListsReferences(T obj, DbTransaction tran)
        {
            if (obj == null)
                return;

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (IsTransiente(prop) || IsSomenteLeitura(prop))
                    continue;

                object prop_value = prop.GetValue(obj, null);

                if (prop_value == null)
                    continue;

                object[] attrs = prop.GetCustomAttributes(true);

                if(attrs != null)
                    foreach(var item in attrs)
                        if(item is DBMappedList)
                        {
                            DBMappedList map = (DBMappedList)item;
                            
                            if (string.IsNullOrEmpty(map.ChildrenColumnIds)
                                || string.IsNullOrEmpty(map.ParentColumnIds)
                                || string.IsNullOrEmpty(map.RelationshipTable))
                                throw new DBBrokerException(string.Format(Resources.ErrorMappedListInfo, new string[] { obj.GetType().FullName, prop.Name }));

                            if (!(prop_value is ICollection))
                                throw new DBBrokerException(string.Format(Resources.ErrorMappedListUsage, new string[]{ obj.GetType().FullName, prop.Name }));
                            
                            // única instrução para fazer a tabela base refletir as referências do list                            
                            long partentId = (long)GetIdValue(obj);

                            ICollection list = (ICollection)prop_value;

                            string sql_stmt = "DELETE FROM " + map.RelationshipTable + " WHERE " + map.ParentColumnIds + " = " + GetIdValue(obj) + ";";

                            object child_id = null;
                            foreach (var child in list)
                                if((child_id = GetIdValue(child)) != null && long.Parse(child_id.ToString()) > 0)
                                    sql_stmt +=  "INSERT INTO " + map.RelationshipTable + "(" + map.ParentColumnIds + ", " + map.ChildrenColumnIds + ") " +
                                                 "VALUES (" + partentId + ", " + GetIdValue(child) + "); ";

                            //if(sql_stmt.Length >= 10)
                            //    sql_stmt = sql_stmt.Substring(0, sql_stmt.Length - 10) + "; ";
                            
                            /* {0} RelationshipTable
                             * {1} ParentId
                             * {2} ParentId (Value)
                             * {3} ChildId
                             * {4} SELECT Stmt (UNION ALL) */

                            //string sql_cmd = 
                            //    list.Count > 0 ?
                            //    "DECLARE @temp TABLE(ParentId INT, ChildId INT); " +
                            //    "INSERT INTO @temp(ParentId, ChildId) {4} " +
                            //    "DELETE FROM {0} WHERE {1} = {2} AND {3} NOT IN ( SELECT ChildId FROM @temp ); " +
                            //    "INSERT INTO {0} ({1}, {3}) " +
                            //    "SELECT t.ParentId, t.ChildId FROM @temp t  " +
                            //    "LEFT JOIN {0} ON {0}.{1} = t.ParentId AND {0}.{3} = t.ChildId " +
                            //    "WHERE {0}.{1} IS NULL; "
                            //    :
                            //    "DELETE FROM {0} WHERE {1} = {2}; ";

                            DBBrokerLive.ExecCmdSQL(sql_stmt, transaction: tran);
                        }
            }
        }

        /// <summary>
        /// Deletes the database record of <typeparamref name="T"/> by primary key.
        /// </summary>
        /// <param name="Id">Primary key value of the record</param>
        public static void Delete(int Id)
        {
            Delete(Id, transaction: null);
        }

        /// <summary>
        /// Deletes the database record of <typeparamref name="T"/> by primary key.
        /// </summary>
        /// <param name="Id">Primary key value of the record</param>
        /// <param name="transaction">The database transaction in which the command should be ran</param>
        public static void Delete(int Id, DbTransaction transaction)
        {
            if (Id == 0)
                return;

            T obj = Activator.CreateInstance<T>();
            DBMappedClass map = GetMapeamento(obj);

            ExecCmdSQL(
                cmdText: "DELETE FROM " + map.Table + " WHERE " + map.PrimaryKey + " = " + Id,
                transaction: transaction);
        }

        /// <summary>
        /// Get the database record of <typeparamref name="T"/> by primary key.
        /// </summary>
        /// <param name="Id">Primary key value of the record</param>
        public static T GetById(int Id)
        {
            T obj = Activator.CreateInstance<T>();
            DBMappedClass map = GetMapeamento(obj);

            IList<T> result = ExecCmdSQL("SELECT * FROM " + map.Table + " WHERE " + map.PrimaryKey + " = " + Id);

            return result.Count == 1 ? result[0] : obj;
        }
        
        /// <summary>
        /// Get all records of <typeparamref name="T"/> in the database.
        /// </summary>
        public static List<T> GetAll()
        {
            return GetAll(null);
        }

        /// <summary>
        /// Get all records of <typeparamref name="T"/> in the database.
        /// </summary>
        /// <param name="orderByColumns">Columns separated by comma to be applied in the ORDER BY clause</param>
        public static List<T> GetAll(string orderByColumns)
        {
            T obj = Activator.CreateInstance<T>();
            DBMappedClass map = GetMapeamento(obj);

            List<T> result = ExecCmdSQL(cmdText: "SELECT * FROM " + map.Table + (!string.IsNullOrEmpty(orderByColumns) ? " ORDER BY " + orderByColumns : ""), commandType: CommandType.Text);

            return result;
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        public static List<T> ExecCmdSQL(string cmdText)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: null, commandType: CommandType.Text, levelOfLoad: 1, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">Parameter used in the specified SQL command or script.</param>
        public static List<T> ExecCmdSQL(string cmdText, DbParameter parameter)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, commandType: CommandType.Text, levelOfLoad: 1, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script</param>
        public static List<T> ExecCmdSQL(string cmdText, List<DbParameter> parameters)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, commandType: CommandType.Text, levelOfLoad: 1, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">Parameter used in the specified SQL command or script.</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        public static List<T> ExecCmdSQL(string cmdText, DbParameter parameter, CommandType commandType)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, commandType: commandType, levelOfLoad: 1, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">Parameter used in the specified SQL command or script.</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        /// <param name="levelOfLoad">Informs how deep DBBroker should go while loading the mapped subproperties of objects</param>
        public static List<T> ExecCmdSQL(string cmdText, DbParameter parameter, CommandType commandType, int levelOfLoad)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, commandType: commandType, levelOfLoad: levelOfLoad, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">Parameter used in the specified SQL command or script.</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        /// <param name="levelOfLoad">Informs how deep DBBroker should go while loading the mapped subproperties of objects</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back</param>
        public static List<T> ExecCmdSQL(string cmdText, DbParameter parameter, CommandType commandType, int levelOfLoad, DbTransaction transaction)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: parameter, parameters: null, commandType: commandType, levelOfLoad: levelOfLoad, transaction: transaction);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        public static List<T> ExecCmdSQL(string cmdText, List<DbParameter> parameters, CommandType commandType)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, commandType: commandType, levelOfLoad: 1, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        /// <param name="levelOfLoad">Informs how deep DBBroker should go while loading the mapped subproperties of objects</param>
        public static List<T> ExecCmdSQL(string cmdText, List<DbParameter> parameters, CommandType commandType, int levelOfLoad)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, commandType: commandType, levelOfLoad: levelOfLoad, transaction: null);
        }

        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameters">Parameters used in the specified SQL command or script</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        /// <param name="levelOfLoad">Informs how deep DBBroker should go while loading the mapped subproperties of objects</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back</param>
        public static List<T> ExecCmdSQL(string cmdText, List<DbParameter> parameters, CommandType commandType, int levelOfLoad, DbTransaction transaction)
        {
            return ExecCmdSQL(cmdText: cmdText, parameter: null, parameters: parameters, commandType: commandType, levelOfLoad: levelOfLoad, transaction: transaction);
        }
        
        /// <summary>
        /// Executes the specified SQL command or script. Then an attempt to load the first result set will be made to transform it in an instance of <see cref="List{T}"/> with the data from rows as instances of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="cmdText">SQL command or script that will be executed.</param>
        /// <param name="parameter">
        ///     <para>Parameter used in the specified SQL command or script.</para>
        ///     <para>As a convenience to avoid the creation of a <see cref="List{DbParameter}"/> instance when only one parameter will be used.</para>
        ///     <para>If informed, the value of '<paramref name="parameters"/>' will be ignored.</para>
        /// </param>
        /// <param name="parameters">Parameters used in the specified SQL command or script</param>
        /// <param name="commandType">Informs the type of the SQL specified</param>
        /// <param name="levelOfLoad">Informs how deep DBBroker should go while loading the mapped subproperties of objects</param>
        /// <param name="transaction">Transaction in which this command should run. If an error is raised, it will be rolled back</param>
        public static List<T> ExecCmdSQL(string cmdText, DbParameter parameter = null, List<DbParameter> parameters = null, CommandType commandType = CommandType.Text, int levelOfLoad = 1, DbTransaction transaction = null)
        {
            RecordsAffected = 0;
            List<T> result = new List<T>();
            
            bool IsExternalTransaction = (transaction != null);

            if (IsExternalTransaction && transaction.Connection == null)
                throw new DBBrokerException(Resources.ErrorTranAssociatedConnectionNull);
            
            DbConnection connection = IsExternalTransaction ? transaction.Connection : Configuration.GetOpenConnection<T>();

            try
            {
                DbCommand command = connection.CreateCommand();
                command.CommandText = cmdText;
                command.CommandType = commandType;
                //no effect: ((Oracle.ManagedDataAccess.Client.OracleCommand)command).UseEdmMapping = true;

                if(IsExternalTransaction)
                    command.Transaction = transaction;

                if (parameter != null)
                    command.Parameters.Add(parameter);
                
                if (parameters != null && parameters.Count > 0)
                    foreach (DbParameter param in parameters)
                        command.Parameters.Add(param);

                DbDataReader reader = command.ExecuteReader();
                RecordsAffected = reader.RecordsAffected;
                List<object> r = (List<object>)ObjectFactory(reader, typeof(T), levelOfLoad, transaction);
                
                foreach (object o in r)
                    result.Add((T)o);

                if (!IsExternalTransaction)
                    connection.Dispose();
                else if (Debugger.IsAttached && transaction != null)
                    Console.WriteLine(Resources.WarningTransactionNotCommited);
            }
            catch (Exception ex)
            {
                if (IsExternalTransaction && transaction != null && transaction.Connection != null)
                    transaction.Rollback();

                if (!IsExternalTransaction && connection != null)
                    connection.Dispose();

                if (Debugger.IsAttached)
                    throw new DBBrokerException(string.Format(Resources.ErrorExecutingSqlCommand, new string[]{ ex.Message, cmdText }), ex);
                else
                    throw new DBBrokerException(Resources.ErrorMessageDefault, ex);
            }

            return result;
        }

        private static object ObjectFactory(DbDataReader reader, Type type, int ProfundidadeDaCarga, DbTransaction transaction)
        {
            List<object> objetos = new List<object>();
            if (!reader.HasRows)
            {
                if (!reader.IsClosed)
                    reader.Close();
                return objetos;
            }

            SupportedDatabases contextDatabase = Configuration.GetByType(type).DatabaseContext;

            /* MAPEAMENTO DO RESULTSET  */
            object obj = null, local_value = null;
            DBMappedClass map = null;
            MapeamentoCarga actual_column = new MapeamentoCarga();
            List<MapeamentoCarga> PropertiesMapping = new List<MapeamentoCarga>();
            int PrimaryKeyOrdinal = -1;
            string column_name = "";
            

            try
            {
                obj = Activator.CreateInstance(type);
                map = GetMapeamento(obj);

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    //Apenas para informar em caso de erro
                    actual_column = new MapeamentoCarga(prop.Name, -1);

                    if (IsTransiente(prop))
                        continue;

                    if (IsListCollection(prop))
                    {
                        PropertiesMapping.Add(new MapeamentoCarga(prop, GetMappedList(prop)));
                        continue;
                    }

                    bool isDomainClass = IsDomainClass(prop);
                    column_name = "";

                    if (GetMapeadoPara(prop) != null)
                        column_name = GetMapeadoPara(prop).ToLower();
                    else if (prop.Name.ToLower().Equals("id"))
                        column_name = map.PrimaryKey.ToLower();
                    else if (isDomainClass)
                        column_name = ("id" + prop.Name).ToLower();
                    else
                        column_name = prop.Name.ToLower();
                
                    for (int i = 0; i < reader.FieldCount; i++)
                        if (prop.Name.ToLower().Equals(reader.GetName(i).ToLower())
                            /* OU: Propriedade que não é de domínio */
                            || (!isDomainClass && column_name.Equals(reader.GetName(i).ToLower()))
                            /* OU: Chave primária 
                            || (column_name.Equals("Id") && map.PrimaryKey.Equals(reader.GetName(i)))*/)
                        {
                            if(column_name == map.PrimaryKey.ToLower())
                                PrimaryKeyOrdinal = i;

                            PropertiesMapping.Add(new MapeamentoCarga(prop.Name, i));
                            break;
                        }
                        else if (isDomainClass && column_name.ToLower().Equals(reader.GetName(i).ToLower()))
                        {
                            PropertiesMapping.Add(new MapeamentoCarga(prop, i, GetMapeamento(prop), GetMapeadoPara(prop)));
                            break;
                        }
                }

                actual_column = new MapeamentoCarga();

                while (reader.Read())
                {
                    obj = Activator.CreateInstance(type);

                    foreach (MapeamentoCarga column in PropertiesMapping)
                    {
                        actual_column = column;

                        if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.Dominio)
                        {
                            // Armazenar dados para recuperação e vinculação posterior, e de uma vez só
                            object local_obj = Activator.CreateInstance(column.Prop.PropertyType);

                            PropertyInfo id_prop = local_obj.GetType().GetProperty("Id");

                            if (id_prop == null)
                                throw new DBBrokerException(string.Format(Resources.ErrorMissingIdProp, local_obj.GetType().FullName));

                            id_prop.SetValue(local_obj, int.Parse(reader.IsDBNull(column.Ordinal) ? "0" : reader.GetValue(column.Ordinal).ToString()), null);

                            SetValue(column.Prop, obj, reader.IsDBNull(column.Ordinal) ? null : local_obj);

                            int id = int.Parse(reader.IsDBNull(column.Ordinal) ? "0" : reader.GetValue(column.Ordinal).ToString());

                            if (id > 0 && !column.IdsChaveEstrangeira.ContainsKey(id))
                                column.IdsChaveEstrangeira.Add(id, local_obj);
                        }
                        else if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.Primitivo)
                        {
                            var local_prop = obj.GetType().GetProperty(column.PropName);
                            if (!reader.IsDBNull(column.Ordinal) && local_prop != null)
                            {
                                local_value = reader.GetValue(column.Ordinal);

                                if (contextDatabase == SupportedDatabases.Oracle)
                                    local_value = OracleParser(local_prop.PropertyType, local_value);

                                SetValue(local_prop, obj, local_value);
                                local_value = null;
                            }
                        }
                        else if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.List)
                        {
                            object id_value = null;
                            if(PrimaryKeyOrdinal > -1 
                                && (id_value = reader.GetValue(PrimaryKeyOrdinal)) != null
                                && !column.IdsChaveEstrangeira.ContainsKey(long.Parse(id_value.ToString())))
                            {
                                column.IdsChaveEstrangeira.Add(long.Parse(id_value.ToString()), null);
                            }
                        }
                    }
                    objetos.Add(obj);
                }

                reader.Close();

                // Carga dos objetos e lists
                foreach (MapeamentoCarga column in PropertiesMapping)
                {
                    actual_column = column;

                    if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.Primitivo)
                        continue;

                    if ((column.TipoCarga == MapeamentoCarga.TipoPropriedade.Dominio || column.TipoCarga == MapeamentoCarga.TipoPropriedade.List)
                        && (column.IdsChaveEstrangeira == null || column.IdsChaveEstrangeira.Count == 0))
                        continue;

                    if (ProfundidadeDaCarga > 0)
                    {
                        if (IgnoreLists && column.TipoCarga == MapeamentoCarga.TipoPropriedade.List)
                            continue;

                        //using (DbConnection conn = Configuration.GetOpenConnection<T>())
                        //{
                            DbConnection conn = transaction != null && transaction.Connection != null ?
                                transaction.Connection :
                                Configuration.GetOpenConnection<T>();
                            
                            DbCommand command = conn.CreateCommand();
                            command.CommandText = column.SqlStmt;
                            command.CommandType = CommandType.Text;

                            if(transaction != null && transaction.Connection != null)
                                command.Transaction = transaction;

                            Type local_type = (column.TipoCarga == MapeamentoCarga.TipoPropriedade.List ? column.TipoDeDominio : column.Prop.PropertyType);
                            List<object> result = (List<object>)ObjectFactory(command.ExecuteReader(), local_type, (ProfundidadeDaCarga - 1), transaction);

                            if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.Dominio)
                            {
                                foreach (object obj_recuperado in result)
                                {
                                    int id = int.Parse(obj_recuperado.GetType().GetProperty("Id").GetValue(obj_recuperado, null).ToString());
                                    if (id > 0)
                                        column.IdsChaveEstrangeira[id] = obj_recuperado;
                                }

                                int i = -1;
                                foreach (object _obj in objetos)
                                {
                                    ++i;
                                    object obj_value = column.Prop.GetValue(_obj, null);

                                    if (obj_value == null)
                                        continue;

                                    object id_value = obj_value.GetType().GetProperty("Id").GetValue(obj_value, null);

                                    long id = 0;
                                    if (id_value != null && long.TryParse(id_value.ToString(), out id))
                                    {
                                        object local_obj = null;
                                        column.IdsChaveEstrangeira.TryGetValue(id, out local_obj);
                                        column.Prop.SetValue(objetos[i], local_obj, null);
                                    }
                                }
                            }
                            else if (column.TipoCarga == MapeamentoCarga.TipoPropriedade.List)
                            {
                                // Preciso ver no banco quais objetos retornados devem ser atribuídos a cada um dos resultado
                                LoadCollectionsVinculated(column, result);

                                foreach (var _obj in objetos)
                                    if (column.IdsChaveEstrangeira.ContainsKey(long.Parse(GetIdValue(_obj).ToString())))
                                        column.Prop.SetValue(_obj, column.IdsChaveEstrangeira[long.Parse(GetIdValue(_obj).ToString())], null);
                            }
                        //}
                    }
                    else if (ProfundidadeDaCarga == 0)
                    {
                        // Fim de papo?... too deep now? :z
                    }
                }
            }
            catch (Exception ex)
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();

                throw new DBBrokerException(string.Format(Resources.ErrorLoadingProp, new string[] { obj != null ? obj.GetType().Name : "null", actual_column.PropName, ex.Message + (local_value != null ? " (" + local_value + ")": "") }), ex);
            }

            return objetos;
        }

        private static object OracleParser(Type targetType, object source_value)
        {
            if (source_value == null)
                return null;

            if (targetType == typeof(string))
                return source_value.ToString();

            if (targetType == typeof(short) || targetType == typeof(short?))
                return short.Parse(source_value.ToString());

            if (targetType == typeof(int) || targetType == typeof(int?))
                return int.Parse(source_value.ToString());

            if (targetType == typeof(long) || targetType == typeof(long?))
                return long.Parse(source_value.ToString());

            if (targetType == typeof(float) || targetType == typeof(float?))
                return float.Parse(source_value.ToString());

            if (targetType == typeof(double) || targetType == typeof(double?))
                return double.Parse(source_value.ToString());

            if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                return decimal.Parse(source_value.ToString());

            if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                return DateTime.Parse(source_value.ToString());

            if (targetType == typeof(bool) || targetType == typeof(bool?))
                return source_value.ToString().Equals("1");

            return null;
        }

        private static void LoadCollectionsVinculated(MapeamentoCarga column, ICollection source)
        {
            if (column.IdsChaveEstrangeira == null || column.IdsChaveEstrangeira.Count == 0 || source == null || source.Count == 0)
                return;

            using (DbConnection conn = Configuration.GetOpenConnection<T>())
            {
                DbCommand sqlcmd = conn.CreateCommand();

                sqlcmd.CommandText = " SELECT " + column.MapeamentoLista.ParentColumnIds + " AS IdParent, " + column.MapeamentoLista.ChildrenColumnIds + " AS IdChild " + 
                                     " FROM " + column.MapeamentoLista.RelationshipTable +
                                     " WHERE " + column.MapeamentoLista.ParentColumnIds + column.InStmtBuilder(column.IdsChaveEstrangeira.Keys);
                sqlcmd.CommandType = CommandType.Text;

                Dictionary<object, object> optimized_source = new Dictionary<object, object>();

                foreach (var item in source)
                    optimized_source.Add(GetIdValue(item), item);

                // Buscar os ids para vinculaçãos
                DbDataReader reader = sqlcmd.ExecuteReader();
                //[0] IdParent
                //[1] IdChild
                while(reader.Read())
                {
                    long id_key = 0;
                    if (reader.IsDBNull(0) || reader.IsDBNull(1) || !long.TryParse(reader[0].ToString(), out id_key))
                        continue;

                    if (column.IdsChaveEstrangeira[id_key] == null)
                    {
                        Type genericDefinition = column.Prop.PropertyType.GetGenericTypeDefinition();
                        IList list = (IList)Activator.CreateInstance(genericDefinition.MakeGenericType(new Type[] { column.TipoDeDominio }));
                        column.IdsChaveEstrangeira[id_key] = list;
                    }

                    if (optimized_source.ContainsKey(reader.GetValue(1)))
                        ((IList)column.IdsChaveEstrangeira[id_key]).Add(optimized_source[reader.GetValue(1)]);
                    else
                        throw new DBBrokerException(string.Format(Resources.ErrorMappedListCheckMapping, new string[] { column.Prop.DeclaringType.FullName, column.PropName }));
                }
            }
        }

        private static bool IsDomainClass(PropertyInfo prop)
        {
            if (string.IsNullOrEmpty(prop.PropertyType.Namespace))
                throw new DBBrokerException(string.Format(Resources.ErrorNamespaceNotFound, ""));

            Configuration value;
            return Configuration.Data.TryGetValue(prop.PropertyType.Namespace, out value);
        }

        private static void LoadParameters(T obj, IList<DbParameter> parametros)
        {
            if (obj == null)
                return;
            
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (prop.Name.ToLower().Equals("id") || IsTransiente(prop) || IsSomenteLeitura(prop) || IsListCollection(prop))
                    continue;

                object prop_value = prop.GetValue(obj, null);
                string mapped_field = (mapped_field = GetMapeadoPara(prop)) != null ? mapped_field : IsDomainClass(prop) ? "Id" + prop.Name : prop.Name;
                
                if (IsDomainClass(prop))
                {
                    object value = prop.GetValue(obj, null);
                    if (value != null)
                        value = GetIdValue(value);
                    parametros.Add(Configuration.GetParameter<T>(mapped_field, value == null || ((long)value) == 0 ? DBNull.Value : value));
                }
                else if(prop.PropertyType.Equals(typeof(byte[])) && prop_value == null)
                {
                    DbParameter param = Configuration.GetParameter<T>(mapped_field, DBNull.Value);
                    param.DbType = DbType.Byte;
                    parametros.Add(param);
                }
                else
                    parametros.Add(Configuration.GetParameter<T>(mapped_field, prop_value));
            }
        }

        private static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }

        private static string GetInsertStmt(T obj)
        {
            DBMappedClass map = GetMapeamento(obj);
            SupportedDatabases contextDatabase = Configuration.GetByType<T>().DatabaseContext;
            string varPointer = contextDatabase == SupportedDatabases.Oracle ? ":" : "@";

            string Stmt_Fields = " INSERT INTO " + map.Table + " (";
            string Stmt_Values = " VALUES( ";

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (IsTransiente(prop) || IsSomenteLeitura(prop) || IsListCollection(prop))
                    continue;

                string mapped_field = (mapped_field = GetMapeadoPara(prop)) != null ? mapped_field : IsDomainClass(prop) ? "Id" + prop.Name : prop.Name;

                if (prop.Name.ToLower().Equals("id"))
                    if (contextDatabase == SupportedDatabases.Oracle)
                    {
                        Stmt_Fields += map.PrimaryKey + ", ";
                        Stmt_Values += !string.IsNullOrEmpty(map.Sequence) ? map.Sequence + ".NEXTVAL, " : ":" + map.PrimaryKey + ", ";
                        continue;
                    }
                    else
                        continue;

                Stmt_Fields += mapped_field + ", ";
                Stmt_Values += varPointer + mapped_field + ", ";
            }

            Stmt_Fields = Stmt_Fields.Substring(0, Stmt_Fields.Length - 2) + ") ";
            Stmt_Values = Stmt_Values.Substring(0, Stmt_Values.Length - 2) + ")"; 

            return Stmt_Fields + Stmt_Values + /*If not Oracle...*/ (contextDatabase != SupportedDatabases.Oracle ? ";" : "");
        }

        private static string GetUpdateStmt(T obj)
        {
            DBMappedClass map = GetMapeamento(obj);
            string varPointer = Configuration.GetByType<T>().DatabaseContext == SupportedDatabases.Oracle ? ":" : "@";

            string Stmt_Fields = " UPDATE " + map.Table + " SET ";

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                if (prop.Name.Equals("Id"))
                    continue;

                if (IsTransiente(prop) || IsSomenteLeitura(prop) || IsListCollection(prop))
                    continue;

                string mapped_field = (mapped_field = GetMapeadoPara(prop)) != null ? mapped_field : IsDomainClass(prop) ? "Id" + prop.Name : prop.Name;

                Stmt_Fields += mapped_field + " = " + varPointer + mapped_field + ", ";
            }

            Stmt_Fields = Stmt_Fields.Substring(0, Stmt_Fields.Length - 2) +
                            " WHERE " + map.PrimaryKey + " = " + GetIdValue(obj);

            return Stmt_Fields;
        }

        private static object GetIdValue(object obj)
        {
            if (obj == null)
                return 0;

            PropertyInfo id_prop = obj.GetType().GetProperty("Id");

            if (id_prop == null)
                throw new DBBrokerException(string.Format(Resources.ErrorMissingIdProp, obj.GetType().Name));

            object prop_value = id_prop.GetValue(obj, null);

            long id = 0;
            if (prop_value == null || !long.TryParse(prop_value.ToString(), out id))
                throw new DBBrokerException(string.Format(Resources.ErrorIdValueNotValid, obj.GetType().Name));
                        
            return prop_value;
        }

        private static void SetIdValue(object obj, object id_value)
        {
            PropertyInfo id_prop = obj.GetType().GetProperty("Id");
            foreach (var prop in obj.GetType().GetProperties())
                if (prop.Name.ToLower() == "id")
                {
                    id_prop = prop;
                    break;
                }

            if(id_prop == null)
                throw new DBBrokerException(string.Format(Resources.ErrorMissingIdProp, new object[]{ obj.GetType().FullName }));

            string str_id_value = id_value != null ? id_value.ToString() : "0";

            short short_id = 0;
            int int_id = 0;
            long long_id = 0;
            
            if(short.TryParse(str_id_value, out short_id) && id_prop.PropertyType == typeof(short))
                id_prop.SetValue(obj, short_id, null);
            else if(int.TryParse(str_id_value, out int_id) && id_prop.PropertyType == typeof(int))
                id_prop.SetValue(obj, int_id, null);
            else if (long.TryParse(str_id_value, out long_id) && id_prop.PropertyType == typeof(long))
                id_prop.SetValue(obj, long_id, null);                        
        }

        private static void SetValue(PropertyInfo prop, object Obj, object value)
        {
            if (value == null)
                return;

            if (value.GetType() == typeof(UInt64))
            {
                prop.SetValue(Obj, int.Parse(value.ToString()), null);
            }
            else
                prop.SetValue(Obj, value, null);
        }

        private static DBMappedClass GetMapeamento(PropertyInfo prop)
        {
            if (prop == null)
                throw new DBBrokerException(Resources.ErrorUnexpectedNullProp);

            object[] attrs = prop.PropertyType.GetCustomAttributes(true);

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBMappedClass))
                        return (DBMappedClass)attr;

            throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, prop.PropertyType.FullName));
        }

        private static DBMappedList GetMappedList(PropertyInfo prop)
        {
            if (prop == null)
                throw new DBBrokerException(Resources.ErrorUnexpectedNullProp);

            if (!prop.PropertyType.FullName.StartsWith("System.Collections.Generic.List"))
                throw new DBBrokerException(string.Format(Resources.ErrorBadList, prop.Name));

            object[] attrs = prop.GetCustomAttributes(true);

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBMappedList))
                        return (DBMappedList)attr;

            throw new DBBrokerException(string.Format(Resources.ErrorMappedListInfo, new object[] { prop.ReflectedType.Name, prop.Name }));
        }

        private static DBMappedClass GetMapeamento(object obj)
        {
            if (obj == null)
                throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, obj.GetType().FullName));

            object[] attrs = obj.GetType().GetCustomAttributes(true);

            DBMappedClass map = null;

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBMappedClass))
                        map = (DBMappedClass)attr;

            if(map == null)
                throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, obj.GetType().FullName));
            if (map != null && (map.Table == null || map.Table.Length == 0 || map.PrimaryKey == null || map.PrimaryKey.Length == 0))
                throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, obj.GetType().FullName));
            else 
                return map;

            throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, obj.GetType().FullName));
        }

        private static bool IsSomenteLeitura(PropertyInfo prop)
        {
            if (prop == null)
                return false;

            object[] attrs = prop.GetCustomAttributes(true);

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBReadOnly))
                        return true;

            return false;
        }

        private static bool IsListCollection(PropertyInfo prop)
        {
            TypeFilter filter = new TypeFilter(MyInterfaceFilter);
            Type[] t = prop.PropertyType.FindInterfaces(filter, "System.Collections.ICollection");
            return (t != null && t.Length == 1 && !prop.PropertyType.IsArray);
        }

        private static bool IsTransiente(PropertyInfo prop)
        {
            if (prop == null)
                return true;

            object[] attrs = prop.GetCustomAttributes(typeof(DBTransient), true);

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBTransient))
                        return true;

            return false;
        }

        private static string GetMapeadoPara(PropertyInfo prop)
        {
            if (prop == null)
                return null;

            object[] attrs = prop.GetCustomAttributes(typeof(DBMappedTo), true);

            if (attrs != null)
                foreach (object attr in attrs)
                    if (attr.GetType() == typeof(DBMappedTo))
                        return ((DBMappedTo)attr).Column;

            return null;
        }
    }    
}
