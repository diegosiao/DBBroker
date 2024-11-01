namespace DbBroker.Cli.Services.Interfaces;

/// <summary>
/// Methods to transform SQL in C#
/// </summary>
public interface ISqlTransformer 
{
    string GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable);
}