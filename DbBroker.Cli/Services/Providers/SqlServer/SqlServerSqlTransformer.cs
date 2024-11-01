

public class SqlServerSqlTransformer : ISqlTransformer
{
    public string GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable)
    {
        switch (databaseType){
            case "uniqueidentifier":
                return isNullable ? "Guid?" : "Guid";
            case "varchar":
                return isNullable ? "string?" : "string";
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