namespace DbBroker.Cli.Model;

public class DbBrokerConfig
{
    public IEnumerable<DbBrokerConfigContext>? Contexts { get; set; }

    public DateTime? LastSynced { get; set; }
}

public class DbBrokerConfigContext
{
    public required string Namespace { get; set; }

    public SupportedDatabaseProviders Provider { get; set; } = SupportedDatabaseProviders.SqlServer;

    public required string ConnectionString { get; set; }

    public IEnumerable<DbBrokerConfigDatabaseTable>? Tables { get; set; }
}

public class DbBrokerConfigDatabaseTable
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? PrimaryKeyColumn { get; set; }

    public required IEnumerable<DbBrokerConfigContextColumn> Columns { get; set; }
}