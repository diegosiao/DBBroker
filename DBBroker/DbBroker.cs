using System.Data.Common;
using System.Threading.Tasks;
using DBBroker.Model;

namespace DbBroker;

public static class DbBroker
{
    public static bool DbInsert<TEntity>(this DbConnection connection, TEntity entity, DbTransaction transaction) where TEntity : DataModelBase
    {
        
        return true;
    }

    public static async Task<bool> DbInsertAsync<TEntity>(TEntity entity, DbTransaction transaction) where TEntity : DataModelBase
    {
        
        return true;
    }

    
}
