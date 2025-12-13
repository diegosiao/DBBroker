using DbBroker.Cli.Model;
using DbBroker.Common;

namespace DbBroker.Cli.Extensions;

public static class ResolversExtensions
{
    public static string GetDbTypeString(this SupportedDatabaseProviders? provider, ColumnDescriptorModel columnDescriptorModel)
    {
        switch (provider)
        {
            case SupportedDatabaseProviders.Oracle:
                return GetOracleDbTypeString(columnDescriptorModel);
            case SupportedDatabaseProviders.SqlServer:
                return GetSqlServerDbTypeString(columnDescriptorModel);
            case SupportedDatabaseProviders.Postgres:
                return GetPostgresDbTypeString(columnDescriptorModel);
            default:
                throw new NotImplementedException($"Provider DbType resolution not implemented for {provider}");
        }
    }

    /// <summary>
    /// https://learn.microsoft.com/en-us/dotnet/api/system.data.sqldbtype?view=net-9.0
    /// </summary>
    /// <param name="columnDescriptorModel"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static string GetSqlServerDbTypeString(ColumnDescriptorModel columnDescriptorModel)
    {
        switch (columnDescriptorModel.DataType.ToLower())
        {
            case "varchar":
                return "SqlDbType.VarChar";
            case "datetime":
                return "SqlDbType.DateTime";
            case "datetime2":
                return "SqlDbType.DateTime2";
            case "date":
                return "SqlDbType.Date";
            case "money":
            case "decimal":
                return "SqlDbType.Money";
            case "uniqueidentifier":
                return "SqlDbType.UniqueIdentifier";
            case "int":
                return "SqlDbType.Int";
            default:
                throw new NotImplementedException();
        }
    }

    private static string GetOracleDbTypeString(ColumnDescriptorModel columnDescriptorModel)
    {
        switch (columnDescriptorModel.DataType.ToLower())
        {
            case "varchar":
            case "varchar2":
            case "nvarchar2":
            case "char":
            case "nchar":
            case "rowid":
            case "long":
                return "OracleDbType.Varchar2";
            case "nclob":
                return "OracleDbType.NClob";
            case "clob":
                return "OracleDbType.Clob";
            case "date":
            case "datetime":
            case "timestamp(6)":
                return "OracleDbType.Date";
            case "raw":
                return "OracleDbType.Raw";
            case "bfile":
            case "blob":
                return "OracleDbType.Blob";
            case "float":
            case "decimal":
            case "money":
            case "number":
                return "OracleDbType.Decimal";
            case "integer":
                return "OracleDbType.Int32";
            default:
                return "OracleDbType.Object";
        }
    }

    private static string GetPostgresDbTypeString(ColumnDescriptorModel columnDescriptorModel)
    {
        switch (columnDescriptorModel.DataType.ToLower())
        {
            case "character varying":
            case "varchar":
            case "text":
            case "char":
            case "character":
            case "name":
                return "NpgsqlDbType.Varchar";
            case "date":
                return "NpgsqlDbType.Date";
            case "timestamp":
            case "timestamp without time zone":
            case "timestamp with time zone":
                return "NpgsqlDbType.Timestamp";
            case "bytea":
                return "NpgsqlDbType.Bytea";
            case "numeric":
            case "decimal":
            case "money":
                return "NpgsqlDbType.Numeric";
            case "integer":
            case "int":
            case "int4":
                return "NpgsqlDbType.Integer";
            case "bigint":
            case "int8":
                return "NpgsqlDbType.Bigint";
            case "smallint":
            case "int2":
                return "NpgsqlDbType.Smallint";
            case "real":
            case "float4":
                return "NpgsqlDbType.Real";
            case "double precision":
            case "float8":
                return "NpgsqlDbType.Double";
            case "boolean":
            case "bool":
                return "NpgsqlDbType.Boolean";
            case "uuid":
                return "NpgsqlDbType.Uuid";
            default:
                return "NpgsqlDbType.Unknown";
        }
    }

    public static string GetProviderClientUsingString(this SupportedDatabaseProviders? provider)
    {
        return provider switch
        {
            SupportedDatabaseProviders.Oracle => "using Oracle.ManagedDataAccess.Client;",
            SupportedDatabaseProviders.SqlServer => "using System.Data;",
            SupportedDatabaseProviders.Postgres =>
@"using Npgsql;
using NpgsqlTypes;",
            _ => throw new NotImplementedException($"Client namespace not implemented for {provider}"),
        };
    }
}
