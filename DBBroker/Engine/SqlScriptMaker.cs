using DBBroker.Mapping;
using DBBroker.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;

namespace DBBroker.Engine
{
    /// <summary>
    /// A helper class that generates the database SQL script of objects that reflects the mapped and specified domain.
    /// </summary>
    public class SqlScriptMaker
    {
        internal static readonly string MySqlFK = "\r\n\r\nALTER TABLE {0}\r\n" +
                    "ADD INDEX idx_{0}_{2} ({2} ASC);\r\n\r\n" +
                    "ALTER TABLE {0}\r\n" +
                    "ADD CONSTRAINT FK_{0}_{2}\r\n" +
                    "FOREIGN KEY ({2}) " +
                    "REFERENCES {1}({3})\r\n" +
                    "ON DELETE NO ACTION \r\n" +
                    "ON UPDATE NO ACTION; ";

        internal static readonly string SQLServerFK = "\r\n\r\nALTER TABLE {0} \r\n" +
                    "ADD CONSTRAINT FK_{0}_{2}\r\n" +
                    "FOREIGN KEY ({2}) " +
                    "REFERENCES {1} \r\n" +
                    "ON DELETE  NO ACTION \r\n" +
                    "ON UPDATE  NO ACTION; ";

        //private string Namespace { get; set; }

        private Configuration ConfigurationContext { get; set; }

        private List<Type> MappedClasses { get; set; }

        private List<TableDefinition> Tabelas { get; set; }

        /// <summary>
        /// Takes the first declared namespace in DBBroker.config file to generate the SQL script to create the objects in database that reflects it.
        /// <para>Using this constructor when more than one namespace is defined will raise a DBBrokerException.</para>
        /// </summary>
        public SqlScriptMaker()
        {
            if (Configuration.Data.Count > 1)
                throw new DBBrokerException(Resources.ErrorScriptMultipleNamespacesMapped);

            foreach (string key in Configuration.Data.Keys)
                ConfigurationContext = Configuration.Data[key];

            MappedClasses = new List<Type>();
            Tabelas = new List<TableDefinition>();
        }

        /// <summary>
        /// Takes the specified context namespace to generate the SQL script to create the objects in database that reflects it.
        /// </summary>
        /// <param name="context">The configuration context that should be used to generate the script. Use <see cref="Configuration.GetByType{T}"/> to get one.</param>
        public SqlScriptMaker(Configuration context)
        {
            if (context == null)
                throw new DBBrokerException(Resources.ErrorConfigurationContextRequired);

            ConfigurationContext = context;
            MappedClasses = new List<Type>();
            Tabelas = new List<TableDefinition>();
        }

        /// <summary>
        /// Gets the generated SQL script of the database objects for the model specified in the construction.
        /// </summary>
        public string GetDatabaseScript()
        {
            int assembly_matchs = 0;
            Assembly modelAssembly = null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (assembly.FullName.StartsWith("Microsoft.")
                    || assembly.FullName.StartsWith("System.")
                    || assembly.FullName.StartsWith("mscorlib")
                    || assembly.FullName == "System")
                    continue;
                else
                    foreach (Type type in assembly.GetTypes())
                        if (type.FullName.StartsWith(ConfigurationContext.Namespace))
                        {
                            modelAssembly = assembly;
                            assembly_matchs++;
                            break;
                        }

            if (assembly_matchs > 1)
                throw new DBBrokerException(string.Format(Resources.MultipleAssembliesWithSameNamespace, ConfigurationContext.Namespace));
            else if (assembly_matchs == 0 || modelAssembly == null)
                throw new DBBrokerException(string.Format(Resources.AssemblyNotFound, ConfigurationContext.Namespace));

            foreach (Type t in modelAssembly.GetTypes())
                if (IsModelClass(t, ConfigurationContext.Namespace))
                    MappedClasses.Add(t);

            return CreateDatabaseScript();
        }

        internal static bool IsModelClass(Type type, string Namespace)
        {
            object[] attr = type.GetCustomAttributes(typeof(DBMappedClass), true);
            return (type.FullName.Equals(Namespace + "." + type.Name) && attr != null && attr.Length == 1);
        }

        /// <summary>
        /// Saves to file the generated SQL script of the database objects specified by the constructor. 
        /// </summary>
        /// <param name="filePath">The path to the file of the generated SQL script. </param>
        public void SaveDatabaseScript(string filePath)
        {
            if (filePath == null)
                filePath = AppDomain.CurrentDomain.BaseDirectory + "DBBroker_GeneratedScript.sql";

            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(GetDatabaseScript());
            writer.Dispose();
        }

        private string CreateDatabaseScript()
        {
            string MainScript = string.Format(Resources.DBBrokerScriptMessage, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            
            try
            {
                foreach (Type t in MappedClasses)
                    Tabelas.Add(new TableDefinition(t, ConfigurationContext.Namespace, ConfigurationContext.DatabaseContext));

                //Tabelas
                foreach (var table in Tabelas)
                {
                    MainScript += "\r\n\r\n" + table.Script;
                    if (!string.IsNullOrEmpty(table.Script))
                        MainScript += table.Warnings;
                }

                //Chaves estrangeiras
                foreach (var table in Tabelas)
                    if (table.ScriptForeignKeys != null)
                        MainScript += table.ScriptForeignKeys;

                //Tabelas de relacionamento
                foreach (var table in Tabelas)
                    if (table.Relationships.Count > 0)
                        foreach (Relationship map in table.Relationships)
                            MainScript += GetRelationshipTableScript(map);
            }
            catch (Exception ex)
            {
                MainScript += "\r\n\r\n\r\n" + "Ooops... " + ex.Message;
            }

            return MainScript;
        }

        private string GetRelationshipTableScript(Relationship relationship)
        {
            SupportedDatabases DatabaseContext = ConfigurationContext.DatabaseContext;
            string TabelaBase = null;
            string Script = "";

            foreach(var table in Tabelas)
                if(relationship.Map.RelationshipTable == table.Map.Table)
                {
                    TabelaBase = table.Map.Table;
                    break;
                }
            
            // Create table... if necessary
            if (TabelaBase == null)
            {
                TabelaBase = DatabaseContext == SupportedDatabases.MySQL ? 
                                relationship.Map.RelationshipTable.ToLower() 
                              : relationship.Map.RelationshipTable;

                Script = "\r\n\r\nCREATE TABLE " + TabelaBase + "(\r\n";
                Script += ConfigurationContext.DatabaseContext == SupportedDatabases.SQLServer ?
                                                    "Id" + TabelaBase + " INT NOT NULL IDENTITY(1, 1) CONSTRAINT PK_" + TabelaBase + " PRIMARY KEY,\r\n"
                                                  : "PRIMARY KEY (Id" + TabelaBase + "),\r\nId" + TabelaBase + " INT NOT NULL AUTO_INCREMENT,\r\n";
                Script += relationship.Map.ParentColumnIds + " INT NOT NULL,\r\n";
                Script += relationship.Map.ChildrenColumnIds + " INT NOT NULL, \r\n";
                Script = Script.Substring(0, Script.Length - 4) + "\r\n);";
            }

            // Parent Foreign key
            string[] args = new string[4];
            args[0] = TabelaBase;
            args[1] = relationship.ParentMap.Table;
            args[2] = relationship.Map.ParentColumnIds;
            args[3] = relationship.ParentMap.PrimaryKey;
            
            if (ConfigurationContext.DatabaseContext == SupportedDatabases.SQLServer)
                Script += string.Format(SqlScriptMaker.SQLServerFK, args);
            else if (ConfigurationContext.DatabaseContext == SupportedDatabases.MySQL)
                Script += string.Format(SqlScriptMaker.MySqlFK, args);
            
            //Children Foreign key
            args[1] = DatabaseContext == SupportedDatabases.MySQL ? relationship.ChildrenMap.Table.ToLower() : relationship.ChildrenMap.Table;
            args[2] = relationship.Map.ChildrenColumnIds;
            args[3] = relationship.ChildrenMap.PrimaryKey;
            
            if (DatabaseContext == SupportedDatabases.SQLServer)
                Script += string.Format(SqlScriptMaker.SQLServerFK, args);
            else if (DatabaseContext == SupportedDatabases.MySQL)
                Script += string.Format(SqlScriptMaker.MySqlFK, args);            
            
            return Script;
        }
        
        internal class TableDefinition
        {
            public DBMappedClass Map { get; set; }

            public string Namespace { get; set; }

            public string Warnings { get; set; }
            
            public string Script { get; private set; }

            public string ScriptForeignKeys { get; set; }

            public List<Relationship> Relationships { get; set; }

            public Type Type { get; set; }

            public SupportedDatabases DatabaseContext { get; private set; }

            /*
            =====
            MySQL
            =====
            CREATE TABLE `persons` (
                `idPerson` INT NOT NULL AUTO_INCREMENT,
                `Name` VARCHAR(120) NOT NULL,
                `BirthDate` DATETIME NULL,
                 PRIMARY KEY (`idPerson`));
            =========
            SQLServer
            =========
            CREATE TABLE Persons(
                IdPerson INT NOT NULL IDENTITY(1,1) 
                    CONSTRAINT PK_Persons PRIMARY KEY,
                Name VARCHAR(120) NOT NULL,
                BirthDate DATETIME);
            */

            public TableDefinition(Type type, string modelNamespace, SupportedDatabases database)
            {
                foreach (var item in type.GetCustomAttributes(true))
                    if (item is DBMappedClass)
                        Map = (DBMappedClass)item;

                if (Map == null)
                    return;

                if (Map.Table == null || Map.Table.Length == 0 || Map.PrimaryKey == null || Map.PrimaryKey.Length == 0)
                    throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, type.FullName));

                Type = type;
                Namespace = modelNamespace;
                DatabaseContext = database;
                Relationships = new List<Relationship>();

                Script += DatabaseContext == SupportedDatabases.SQLServer ?
                                                    "CREATE TABLE " + Map.Table + "(\r\n" + Map.PrimaryKey + " INT NOT NULL IDENTITY(1, 1) CONSTRAINT PK_" + Map.Table + " PRIMARY KEY,\r\n"
                                                  : "CREATE TABLE " + Map.Table.ToLower() + "(\r\nPRIMARY KEY (" + Map.PrimaryKey +  "),\r\n" + Map.PrimaryKey + " INT NOT NULL AUTO_INCREMENT,\r\n";
                foreach (PropertyInfo prop in type.GetProperties())
                {
                    if (IsTransiente(prop))
                        continue;

                    if (IsModelClass(prop.PropertyType, Namespace))
                        ScriptForeignKeys += ForeignKey(Map, prop);
                    else if (IsMappedList(prop))
                    {
                        Type[] generic_type = prop.PropertyType.GetGenericArguments();

                        if (generic_type == null || generic_type.Length == 0)
                            throw new DBBrokerException(string.Format(Resources.ErrorBadList, new object[] { prop.PropertyType.FullName, prop.Name }));

                        Relationships.Add(new Relationship((DBMappedList)GetPropMap(prop, typeof(DBMappedList)), GetClassMap(type), GetClassMap(generic_type[0])));
                        continue;
                    }

                    if(prop.Name.ToLower() == "id")
                        continue;

                    string dbcolumn = PropConversion(prop);

                    if (dbcolumn == null)
                    {
                        Warnings += "\r\n/* " + string.Format(Resources.WarningPropertyNotMapped, new object[]{ type.Name + "." + prop.Name, prop.PropertyType.Name }) + " */";
                        continue;
                    }

                    DBReadOnly read_only = (DBReadOnly)GetPropMap(prop, typeof(DBReadOnly));
                    if (read_only != null && !string.IsNullOrEmpty(read_only.DBDefaultValue))
                    {
                        Script += dbcolumn;
                        Script += " DEFAULT " + read_only.DBDefaultValue + ", \r\n";
                        continue;
                    }
                    else if (read_only != null)
                        continue;

                    Script += dbcolumn + ", \r\n";
                }

                Script = Script.Substring(0, Script.Length - 4) + ");";
            }

            private string PropConversion(PropertyInfo prop)
            {
                string sql = "";
                Map map = null;

                // Definir o tipo
                if (prop.PropertyType == typeof(int))
                    sql += "INT NOT NULL";
                else if (prop.PropertyType == typeof(int?))
                    sql += "INT";
                else if (prop.PropertyType == typeof(string))
                    sql += "VARCHAR(120)";
                else if (prop.PropertyType == typeof(double))
                    sql += "REAL NOT NULL";
                else if (prop.PropertyType == typeof(double?))
                    sql += "REAL";
                else if (prop.PropertyType == typeof(float))
                    sql += "REAL NOT NULL";
                else if (prop.PropertyType == typeof(float?))
                    sql += "REAL";
                else if (prop.PropertyType == typeof(decimal))
                    sql += "MONEY NOT NULL";
                else if (prop.PropertyType == typeof(decimal?))
                    sql += "MONEY";
                else if (prop.PropertyType == typeof(DateTime))
                    sql += "DATETIME NOT NULL";
                else if (prop.PropertyType == typeof(DateTime?))
                    sql += "DATETIME";
                else if (prop.PropertyType == typeof(long))
                    sql += "BIGINT NOT NULL";
                else if (prop.PropertyType == typeof(long?))
                    sql += "BIGINT";
                else if (prop.PropertyType == typeof(bool))
                    sql += "BIT NOT NULL";
                else if (prop.PropertyType == typeof(bool?))
                    sql += "BIT";
                else if (prop.PropertyType == typeof(byte[]))
                    sql += DatabaseContext == SupportedDatabases.SQLServer ? "VARBINARY(MAX)" : "VARBINARY(512000)";
                else if (IsModelClass(prop.PropertyType, Namespace))
                    sql += "INT";
                                
                if(IsModelClass(prop.PropertyType, Namespace))
                {
                    string col = "";

                    if ((map = GetPropMap(prop, typeof(DBMappedTo))) != null)
                        col = ((DBMappedTo)map).Column;
                    else
                        col = "Id" + prop.Name;

                    sql = col + " " + sql;
                }
                else if ((map = GetPropMap(prop, typeof(DBMappedTo))) != null)
                    sql = ((DBMappedTo)map).Column + " " + sql;
                else if(sql == "")
                    return null;
                else 
                    sql = prop.Name + " " + sql;
                
                return sql;
            }

            private string ForeignKey(DBMappedClass map, PropertyInfo prop)
            {
                DBMappedClass referenceMap = GetClassMap(prop.PropertyType);
                Map propReference = null;

                string[] args = new string[] {  (DatabaseContext == SupportedDatabases.MySQL ? map.Table.ToLower() : map.Table)
                                                ,referenceMap.Table
                                                ,((propReference = GetPropMap(prop, typeof(DBMappedTo))) != null ? ((DBMappedTo)propReference).Column : "Id" + prop.Name)
                                                , referenceMap.PrimaryKey};
                
                if (DatabaseContext == SupportedDatabases.SQLServer)
                    return string.Format(SqlScriptMaker.SQLServerFK, args);
                else if (DatabaseContext == SupportedDatabases.MySQL)
                    return string.Format(SqlScriptMaker.MySqlFK, args);
                else
                    throw new DBBrokerException("Unknown database context. ");
            }

            private bool IsReadOnly(PropertyInfo prop)
            {
                return (GetPropMap(prop, typeof(DBReadOnly))) != null;
            }

            private bool IsTransiente(PropertyInfo prop)
            {
                return (GetPropMap(prop, typeof(DBTransient))) != null;
            }

            private bool IsMappedList(PropertyInfo prop)
            {
                return (GetPropMap(prop, typeof(DBMappedList))) != null;
            }

            private DBMappedClass GetClassMap(Type type)
            {
                object[] custom_attr = null;

                if(type == null || (custom_attr = type.GetCustomAttributes(true)) == null)
                    return null;

                foreach (var attr in custom_attr)
                    if (attr is DBMappedClass)
                        return (DBMappedClass)attr;

                return null;
            }

            private Map GetPropMap(PropertyInfo prop, Type typeOfMap)
            {
                object[] custom_attr = null;

                if(prop == null || typeOfMap == null || (custom_attr = prop.GetCustomAttributes(true)) == null)
                    return null;

                foreach (var attr in custom_attr)
                    if (attr.GetType() == typeOfMap)
                        return (Map)attr;

                return null;
            }
        }

        internal class Relationship
        {
            public DBMappedList Map { get; set; }
            public DBMappedClass ParentMap { get; set; }
            public DBMappedClass ChildrenMap { get; set; }

            public Relationship(DBMappedList map, DBMappedClass parentMap, DBMappedClass childrenMap)
            {
                Map = map;
                ParentMap = parentMap;
                ChildrenMap = childrenMap;
            }
        }
    }
}
