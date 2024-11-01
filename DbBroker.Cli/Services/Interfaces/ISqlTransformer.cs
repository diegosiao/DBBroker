public interface ISqlTransformer 
{
    string GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable);
}