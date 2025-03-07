namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerMetadataProvider : MetadataProvider
{
    public SqlServerMetadataProvider() : base(SqlServerConstants.SELECT_TABLES_COLUMNS, SqlServerConstants.SELECT_VIEWS_COLUMNS, SqlServerConstants.SELECT_KEYS)
    {
    }
}
