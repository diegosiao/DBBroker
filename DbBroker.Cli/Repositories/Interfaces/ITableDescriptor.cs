using System;
using DbBroker.Cli.Model;

namespace DbBroker.Cli.Repositories.Interfaces;

public interface ITableDescriptor
{
    Task<IEnumerable<DbBrokerConfigContextColumn>> GetColumns(DbBrokerConfigContext context);
}
