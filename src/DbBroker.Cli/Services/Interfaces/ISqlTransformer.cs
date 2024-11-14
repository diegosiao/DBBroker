namespace DbBroker.Cli.Services.Interfaces;

/// <summary>
/// Methods to transform SQL in C#
/// </summary>
public interface ISqlTransformer 
{
    /// <summary>
    /// Get the correspondent C# type for the specified database type.
    /// </summary>
    /// <param name="databaseType">The database type</param>
    /// <param name="databaseTypeLength">The database type length or option</param>
    /// <param name="isNullable">True if the column accepts null, false otherwise</param>
    /// <returns>The C# type name or null if the database type is unsupported</returns>
    string? GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable);
}