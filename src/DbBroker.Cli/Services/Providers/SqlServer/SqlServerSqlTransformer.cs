using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerSqlTransformer : ISqlTransformer
{
    public string? GetCSharpType(string databaseType, string? databaseTypeLength, bool isNullable)
    {
        return databaseType.ToLower() switch
        {
            "uniqueidentifier" => isNullable ? "Guid?" : "Guid",
            "varchar" or "char" => "string?",
            "date" or "datetime" => isNullable ? "DateTime?" : "DateTime",
            "decimal" or "money" => isNullable ? "decimal?" : "decimal",
            "int" => isNullable ? "int?" : "int",
            _ => null,
        };
    }
}