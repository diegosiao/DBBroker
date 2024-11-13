using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Interfaces;

public interface ICSharpClassGenerator
{
    Task<int> GenerateAsync(DbBrokerConfigContext configurationFiles);
}
