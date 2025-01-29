using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleSqlTransformer : ISqlTransformer
{
    /// <summary>
    /// Tries to map an Oracle database type to a compatible .NET type. See 
    /// <a href='https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/oracle-data-type-mappings'>More information.</a>
    /// </summary>
    /// <param name="databaseType">The database type name</param>
    /// <param name="databaseTypeLength">The database type length</param>
    /// <param name="isNullable">Specifies if the column associated with the type accepts null values</param>
    /// <returns>The .NET type short name or object if no mapping is found.</returns>
    public string? GetCSharpType(string databaseType, string? databaseTypeLength, bool isNullable)
    {
        switch (databaseType.ToLower())
        {
            case "varchar":
            case "varchar2":
            case "nvarchar2":
            case "char":
            case "clob":
            case "rowid":
                return "string?";
            case "date":
            case "datetime":
            case "timestamp(6)":
                return isNullable ? "DateTime?" : "DateTime";
            case "raw":
            case "bfile":
            case "blob":
                return "byte[]?";
            case "float":
            case "decimal":
            case "money":
            case "number":
                return isNullable ? "decimal?" : "decimal";
            case "integer":
                return isNullable ? "int?" : "int";
            default:
                return "object";
        }
    }
}
