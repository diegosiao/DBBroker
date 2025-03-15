using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleDefaultConfiguration : IProviderDefaultConfiguration
{
    public string ISqlInsertTemplateTypeFullName => "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
}
