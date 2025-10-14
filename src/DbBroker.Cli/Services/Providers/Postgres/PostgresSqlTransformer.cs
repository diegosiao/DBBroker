using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Postgres;

public class PostgresSqlTransformer : ISqlTransformer
{
    public string? GetCSharpType(string databaseType, string? databaseTypeLength, string? dataPrecision, string? dataScale, bool isNullable)
    {
        switch (databaseType.ToLower())
        {
            case "character varying":
            case "varchar":
            case "text":
            case "char":
            case "character":
            case "name":
                return "string?";
            case "timestamp":
            case "timestamp without time zone":
            case "timestamp with time zone":
            case "date":
                return isNullable ? "DateTime?" : "DateTime";
            case "bytea":
                return "byte[]?";
            case "numeric":
            case "decimal":
            case "money":
                return isNullable ? "decimal?" : "decimal";
            case "integer":
            case "int":
            case "int4":
                return isNullable ? "int?" : "int";
            case "bigint":
            case "int8":
                return isNullable ? "long?" : "long";
            case "smallint":
            case "int2":
                return isNullable ? "short?" : "short";
            case "real":
            case "float4":
                return isNullable ? "float?" : "float";
            case "double precision":
            case "float8":
                return isNullable ? "double?" : "double";
            case "boolean":
            case "bool":
                return isNullable ? "bool?" : "bool";
            case "uuid":
                return isNullable ? "Guid?" : "Guid";
            default:
                return "object";
        }
    }
}
