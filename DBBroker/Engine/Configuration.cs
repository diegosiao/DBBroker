using DBBroker.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DBBroker.Engine
{
    /// <summary>
    /// This class holds information of the DBBroker.config file.
    /// </summary>
    public class Configuration
    {
        private static string ConfigFileHeaderKey = "[DBBroker_Config]->[Encrypted_File]#[DO_NOT_CHANGE_THIS_FILE]#\r\n";
        
        /// <summary>
        /// The required password used to encrypt and decrypt the DBBroker.config file.
        /// <para>The password informed in the creation or encryption of the DBBroker.config file must match to the one informed in this property before any database interaction.</para>
        /// </summary>
        public static SecureString EncryptionPassword;

        /// <summary>
        /// The mapped namespace of this configuration context.
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// Line of this configuration in file
        /// </summary>
        public int Line { get; private set; }

        /// <summary>
        /// The database context of the namespace.
        /// </summary>
        public SupportedDatabases DatabaseContext { get; private set; }

        /// <summary>
        /// The connection string associated with the respective namespace.
        /// </summary>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the configuration instance that represents the specified line in DBBroker.config file.
        /// </summary>
        /// <param name="Line">Number of the line in DBBroker.config</param>
        internal static Configuration GetByLine(int Line)
        {
            if (Line <= 0)
                Line = 1;

            if (Line > Data.Count)
                throw new DBBrokerException(string.Format(Resources.ErrorConfigFileHasNoSuchLine, Line, Data.Count));

            foreach (Configuration item in Data.Values)
                if (item.Line == Line)
                    return item;

            return null;
        }

        /// <summary>
        /// Gets the configuration instance that represents the line in DBBroker.config file for the namespace of the type informed.
        /// </summary>
        /// <typeparam name="T">Any type from the namespace of the desired line in DBBroker.config file</typeparam>
        public static Configuration GetByType<T>()
        {
            if (!Data.ContainsKey(typeof(T).Namespace))
                throw new DBBrokerException(string.Format(Resources.ErrorNamespaceNotFound, typeof(T).Namespace));

            return Data[typeof(T).Namespace];
        }

        internal static Configuration GetByType(Type type)
        {
            if (!Data.ContainsKey(type.Namespace))
                throw new DBBrokerException(string.Format(Resources.ErrorNamespaceNotFound, type.Namespace));

            return Data[type.Namespace];
        }

        private Configuration() {  }

        /// <summary>
        /// Clear all existing configuration values and reloads them from DBBroker.config file.
        /// </summary>
        public static void Reload()
        {
            LoadDBBrokerConfig();
        }

        private static void LoadDBBrokerConfig()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
                throw new Exception(string.Format(Resources.DBBrokerConfigFileNotFound, AppDomain.CurrentDomain.BaseDirectory));

            _data = null;

            Dictionary<string, Configuration> local_data = new Dictionary<string, Configuration>();

            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
            {
                string content = reader.ReadToEnd();

                if (content == null || content.Length < 10)
                    throw new DBBrokerException(Resources.DBBrokerConfigFileFormat);

                string decrypted_config = null;
                if (content.StartsWith(ConfigFileHeaderKey))
                {
                    decrypted_config = Decrypt(content.Substring(ConfigFileHeaderKey.Length));
                    content = decrypted_config;
                }

                string[] linhas = content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                string[] configs = null;

                int line = 0;
                foreach (var linha in linhas)
                {
                    configs = linha.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    if (configs.Length != 3 && !string.IsNullOrEmpty(decrypted_config))
                        throw new DBBrokerException(Resources.EncryptionPasswordInvalid);
                    else if (configs.Length != 3)
                        throw new DBBrokerException(Resources.DBBrokerConfigFileFormat);

                    Configuration config = new Configuration();
                    config.Line = ++line;
                    config.Namespace = configs[0];
                    config.ConnectionString = configs[2];

                    string databaseoption = configs[1].ToLower();

                    if (!"sqlserver".Equals(databaseoption) && !"mysql".Equals(databaseoption) && !"oracle".Equals(databaseoption))
                        if(!string.IsNullOrEmpty(decrypted_config))
                            throw new DBBrokerException(Resources.EncryptionPasswordInvalid);
                        else 
                            throw new DBBrokerException(Resources.DBBrokerConfigDatabaseOption);

                    if (databaseoption.Equals("sqlserver"))
                        config.DatabaseContext = SupportedDatabases.SQLServer;
                    else if (databaseoption.Equals("mysql"))
                        config.DatabaseContext = SupportedDatabases.MySQL;
                    else if (databaseoption.Equals("oracle"))
                        config.DatabaseContext = SupportedDatabases.Oracle;

                    if (local_data.ContainsKey(config.Namespace))
                        throw new DBBrokerException(string.Format(Resources.ErrorNamespaceAlreadyInformed, config.Namespace));

                    local_data.Add(config.Namespace, config);
                }
            }

            _data = local_data;
        }

        private static Dictionary<string, Configuration> _data;

        /// <summary>
        /// Holds every line of DBBroker.config file. Use the namespace as Key.
        /// </summary>
        internal static Dictionary<string, Configuration> Data
        {
            get
            {
                if (_data == null)
                {
                    LoadDBBrokerConfig();
                }
                return _data;
            }
        }

        /// <summary>
        /// Number of configuration lines in DBBroker.config file.
        /// </summary>
        public static int Count
        {
            get
            {
                return Data.Count;
            }
        }

        /// <summary>
        /// Gets an open database connection based on configuration of the namespace of the type specified. 
        /// </summary>
        /// <typeparam name="T">Type of the connection context</typeparam>
        public static DbConnection GetOpenConnection<T>()
        {
            return GetOpenConnection(GetByType<T>());
        }

        /// <summary>
        /// Gets an open database connection based on configuration specified. 
        /// </summary>
        /// <param name="context">
        ///     <para>The configuration context specified in DBBroker.config file from which this connection should be generated.</para>
        ///     <para>You can get one using <see cref="Configuration.GetByType{T}"/>. If null, the first line of the DBBroker.config file is assumed.</para>
        /// </param>
        internal static DbConnection GetOpenConnection(Configuration context)
        {
            if (context == null)
                context = GetByLine(1);

            DbConnection connection = null;

            switch (context.DatabaseContext)
            {
                case SupportedDatabases.SQLServer:
                    (connection = new SqlConnection(context.ConnectionString)).Open();
                    break;
                case SupportedDatabases.MySQL:
                    (connection = new MySqlConnection(context.ConnectionString)).Open();
                    break;
                //case SupportedDatabases.Oracle:
                //    (connection = new OracleConnection(context.ConnectionString)).Open();
                //    break;
            }

            return connection;
        }

        internal static DbParameter GetParameter<T>(string name, object value)
        {
            switch (GetByType<T>().DatabaseContext)
            {
                case SupportedDatabases.SQLServer:
                    return new SqlParameter(name, value != null ? value : DBNull.Value);
                case SupportedDatabases.MySQL:
                    return new MySqlParameter(name, value != null ? value : DBNull.Value);
                //case SupportedDatabases.Oracle:
                //    if (value == null)
                //        return new OracleParameter(name, DBNull.Value);
                //    else if (value is string)
                //        return new OracleParameter(name, OracleDbType.Varchar2) { Value = value };

                //    // Counting on DB server truncation instead of an error
                //    else if (value is short || value is int || value is Int32 || value is long)
                //        return new OracleParameter(name, OracleDbType.Long) { Value = value };

                //    // Counting on DB server truncation instead of an error
                //    else if (value is decimal || value is float || value is double)
                //        return new OracleParameter(name, OracleDbType.Double) { Value = value };                    

                //    else if (value is DateTime)
                //        return new OracleParameter(name, OracleDbType.Date) { Value = value };

                //    else if (value is bool)
                //        return new OracleParameter(name, OracleDbType.Int16) { Value = (((bool)value) ? 1 : 0) }; //Number(1)

                //    else
                //        return new OracleParameter(name, value);
            }

            throw new DBBrokerException(Resources.DBBrokerConfigDatabaseOption + " [DB Context]");
        }

        //internal static OracleDbType GetOracleDbType(object value)
        //{
        //    if(value is bool)
        //        return OracleDbType.Char;

        //    if (value is string)
        //        return OracleDbType.Varchar2;

        //    return OracleDbType.Varchar2;
        //}

        /// <summary>
        /// ATTENTION: For Oracle context an empty sequence name will get a function returning '0'
        /// </summary>
        /// <typeparam name="T">The context type for this operation</typeparam>
        /// <param name="map">For Oracle context inform the Sequence</param>
        internal static string GetLastInsertedIdFunction<T>(DBBroker.Mapping.DBMappedClass map)
        {
            switch (GetByType<T>().DatabaseContext)
            {
                case SupportedDatabases.SQLServer:
                    return "SELECT CAST(COALESCE(@@IDENTITY, 0) AS INT) AS " + map.PrimaryKey + ";";
                case SupportedDatabases.MySQL:
                    return "SELECT LAST_INSERT_ID() AS " + map.PrimaryKey + " FROM " + map.Table + ";";
                case SupportedDatabases.Oracle:
                    return (string.IsNullOrEmpty(map.Sequence) ?
                        "SELECT 0 AS " + map.PrimaryKey + " FROM DUAL"
                        : "SELECT " + map.Sequence + ".CURRVAL AS " + map.PrimaryKey + " FROM DUAL");
            }

            throw new DBBrokerException(Resources.DBBrokerConfigDatabaseOption + " [DB Context]");
        }

        #region Encription

        /// <summary>
        /// Encrypts the specified string array as lines of the 'DBBroker.config' file considering the value of <see cref="Configuration.EncryptionPassword"/> as the password. The same password is required for decryption when loading configuration values.
        /// </summary>
        /// <param name="configurationLines">The line(s) content(s) of the DBBroker.config file.
        /// <para>Each line in format:</para>
        /// <para>Mapped namespace|SQLServer|Connection string</para>
        /// </param>
        public static void EncryptConfigFile(string[] configurationLines)
        {
            if (configurationLines == null || configurationLines.Length == 0)
                throw new Exception(Resources.DBBrokerConfigInfoIncomplete);

            string content = "";
            foreach (var item in configurationLines)
                content += item + "\r\n";

            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
            {
                content = ConfigFileHeaderKey + Encrypt(content);
                writer.Write(content);
            }
        }

        /// <summary>
        /// Encrypts the contents of the existing 'DBBroker.config' file considering the value of <see cref="Configuration.EncryptionPassword"/> as the password. The same password is required for decryption when loading configuration values.
        /// <para></para>
        /// </summary>
        public static void EncryptConfigFile()
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
                throw new Exception(string.Format(Resources.DBBrokerConfigFileNotFound, AppDomain.CurrentDomain.BaseDirectory));

            StreamReader reader = null;
            string content = "";
            using (reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
            {
                content = reader.ReadToEnd();

                if (!content.StartsWith(ConfigFileHeaderKey))
                    content = Encrypt(content);
                else
                    throw new Exception(Resources.DBBrokerConfigFileAlreadyEncripted);
            }
            
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "DBBroker.config"))
            {
                content = ConfigFileHeaderKey + content;
                writer.Write(content);
            }
        }

        private static readonly string PasswordHash = "m3u_sup3r_p@$3w0rd_diego_rocks!_just_kidding:)";
        private static readonly string SaltKey = "minh4_+++=S@LT&KEY";
        private static readonly string VIKey = "@1B2c3D4e5F6g7H8";

        private static string convertToUnSecureString(SecureString secure_string)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secure_string);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private static string Encrypt(string texto)
        {
            byte[] texto_bytes = Encoding.UTF8.GetBytes(texto);

            if (EncryptionPassword == null || EncryptionPassword.Length < 6)
                throw new DBBrokerException(Resources.EncryptionPasswordNull);

            string local_s = PasswordHash + convertToUnSecureString(EncryptionPassword);

            byte[] key_bytes = new Rfc2898DeriveBytes(local_s, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(key_bytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] texto_encriptado_bytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(texto_bytes, 0, texto_bytes.Length);
                    cryptoStream.FlushFinalBlock();
                    texto_encriptado_bytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(texto_encriptado_bytes);
        }

        private static string Decrypt(string texto_encriptado)
        {
            if (EncryptionPassword == null || EncryptionPassword.Length < 6)
                throw new DBBrokerException(Resources.EncryptionPasswordNull);

            byte[] texto_encriptado_bytes = Convert.FromBase64String(texto_encriptado);
            
            string local_s = PasswordHash + convertToUnSecureString(EncryptionPassword);

            byte[] key_bytes = new Rfc2898DeriveBytes(local_s, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetric_key = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetric_key.CreateDecryptor(key_bytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(texto_encriptado_bytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] texto_decriptado = new byte[texto_encriptado_bytes.Length];

            int texto_decriptado_length = cryptoStream.Read(texto_decriptado, 0, texto_decriptado.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(texto_decriptado, 0, texto_decriptado_length).TrimEnd("\0".ToCharArray());
        } 
        #endregion
    }

    /// <summary>
    /// SQL Server is the only fully supported database of this project under GNU LICENSE (MySQL not officially supported)
    /// <para>Please refer to http://www.getdbbroker.com/Home/License </para>
    /// </summary>
    public enum SupportedDatabases 
    {
        /// <summary>
        /// The only officially supported database under GNU LICENSE
        /// </summary>
        SQLServer,
        /// <summary>
        /// Future versions will probably fully support this database engine. For now, use with caution.
        /// </summary>
        MySQL,
        /// <summary>
        /// Future versions will probably fully support this database engine. For now, use with caution.
        /// </summary>
        Oracle
    }

}
