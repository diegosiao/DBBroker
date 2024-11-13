namespace DbBroker.Cli.Services.Providers;

using DbBroker.Cli.Services.Interfaces;

public class SqlServerSqlTransformer : ISqlTransformer
{
    public string GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable)
    {
        switch (databaseType.ToLower()){
            case "uniqueidentifier":
                return isNullable ? "Guid?" : "Guid";
            case "varchar":
                return "string?";
            case "date":
            case "datetime":
                return isNullable ? "DateTime?" : "DateTime";
            case "decimal":
            case "money":
                return isNullable ? "decimal?" : "decimal";
            case "int":
                return isNullable ? "int?" : "int";
            default:
                throw new InvalidOperationException($"Database type unsupported: {databaseType}");
        }
    }
}