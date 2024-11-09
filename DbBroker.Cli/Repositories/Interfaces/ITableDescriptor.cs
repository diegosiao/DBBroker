using DbBroker.Common.Model;

namespace DbBroker.Cli.Repositories.Interfaces;

public interface ITableDescriptor
{
    Task<IEnumerable<DbBrokerConfigContextColumn>> GetColumns(DbBrokerConfigContext context);
}
