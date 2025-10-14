using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Postgres;

public class PostgresDefaultConfiguration : IProviderDefaultConfiguration
{
    public string ISqlInsertTemplateTypeFullName => "DbBroker.Model.Providers.Postgres.PostgresExplicitKeySqlInsertTemplate";
}
