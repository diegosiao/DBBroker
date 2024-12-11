namespace DbBroker.Cli.Services.Providers;

using DbBroker.Cli.Services.Interfaces;

public class SqlServerSqlTransformer : ISqlTransformer
{
    public string? GetCSharpType(string databaseType, string? databaseTypeLength, bool isNullable)
    {
        return databaseType.ToLower() switch
        {
            "uniqueidentifier" => isNullable ? "Guid?" : "Guid",
            "varchar" => "string?",
            "date" or "datetime" => isNullable ? "DateTime?" : "DateTime",
            "decimal" or "money" => isNullable ? "decimal?" : "decimal",
            "int" => isNullable ? "int?" : "int",
            _ => null,
        };
    }
}