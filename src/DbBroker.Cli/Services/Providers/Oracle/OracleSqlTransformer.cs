using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleSqlTransformer : ISqlTransformer
{
    public OracleSqlTransformer()
    {
    }

    public string GetCSharpType(string databaseType, string databaseTypeLength, bool isNullable)
    {
        throw new NotImplementedException();
    }
}
