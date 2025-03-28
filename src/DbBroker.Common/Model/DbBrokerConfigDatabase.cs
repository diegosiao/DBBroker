namespace DbBroker.Common.Model;

public class DbBrokerConfigDatabase
{
    public SupportedDatabaseProviders? Provider { get; set; }

    public string ConnectionString { get; set; }
}
