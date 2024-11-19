using DbBroker.Cli.Extensions;
using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleSqlTransformer : ISqlTransformer
{
    public string? GetCSharpType(string databaseType, string? databaseTypeLength, bool isNullable)
    {
        switch (databaseType.ToLower())
        {
            case "varchar":
            case "varchar2":
                return "string?";
            case "date":
            case "datetime":
                return isNullable ? "DateTime?" : "DateTime";
            case "raw":
                return isNullable ? "byte[]?" : "byte[]";
            case "decimal":
            case "money":
                return isNullable ? "decimal?" : "decimal";
            case "integer":
                return isNullable ? "int?" : "int";
            default:
                return "object";
        }
    }
}
