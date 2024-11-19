namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerMetadataProvider : MetadataProvider
{
    public SqlServerMetadataProvider() : base(SqlServerConstants.SELECT_COLUMNS, SqlServerConstants.SELECT_KEYS)
    {
    }
}
