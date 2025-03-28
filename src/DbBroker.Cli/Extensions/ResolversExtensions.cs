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

            default:
                throw new NotImplementedException($"Provider DbType resolution not implemented for {provider}");
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

    public static string GetProviderClientUsingString(this SupportedDatabaseProviders? provider)
    {
        return provider switch
        {
            SupportedDatabaseProviders.Oracle => "using Oracle.ManagedDataAccess.Client;",
            _ => throw new NotImplementedException($"Client namespace not implemented for {provider}"),
        };
    }
}
