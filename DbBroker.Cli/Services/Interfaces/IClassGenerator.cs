using DbBroker.Cli.Model;

namespace DbBroker.Cli.Services.Interfaces;

public interface IClassGenerator
{
    Task<int> GenerateAsync(DbBrokerConfigContext configurationFiles);
}
