namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleMetadaProvider : MetadataProvider
{
    public OracleMetadaProvider() : base(OracleConstants.SELECT_TABLES_COLUMNS, OracleConstants.SELECT_VIEWS_COLUMNS, OracleConstants.SELECT_KEYS)
    {
    }
}
