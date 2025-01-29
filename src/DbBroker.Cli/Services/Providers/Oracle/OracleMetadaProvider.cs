namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleMetadaProvider : MetadataProvider
{
    public OracleMetadaProvider() : base(OracleConstants.SELECT_COLUMNS, OracleConstants.SELECT_KEYS)
    {
    }
}
