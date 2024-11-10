using System;
using DbBroker.Cli.Services.Interfaces;

namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleDefaultConfiguration : IProviderDefaultConfiguration
{
    public string ISqlInsertTemplateTypeFullName => throw new NotImplementedException();
}
