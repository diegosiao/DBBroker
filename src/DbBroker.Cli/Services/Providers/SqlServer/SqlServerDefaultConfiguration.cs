using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerDefaultConfiguration : IProviderDefaultConfiguration
{
    public string ISqlInsertTemplateTypeFullName => "DbBroker.Model.Providers.SqlServerIdentityKeySqlInsertTemplate";
}
