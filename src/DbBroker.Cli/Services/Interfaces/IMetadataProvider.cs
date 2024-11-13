using System.Data.Common;
using DbBroker.Cli.Model;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Interfaces;

public interface IMetadataProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="connection">The Database connection</param>
    /// <param name="context">The configuration context</param>
    /// <returns>A dictionary where the Key is 'Schema.TableName' of the tables and the value contains a <see cref="TableDescriptorModel"/> </returns>
    Task<Dictionary<string, TableDescriptorModel>> GetTableDescriptorsAsync(DbConnection connection, DbBrokerConfigContext context);
}
