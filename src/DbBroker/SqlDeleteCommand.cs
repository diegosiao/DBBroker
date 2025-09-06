using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

/// <summary>
/// SQL DELETE command
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class SqlDeleteCommand<TDataModel> : SqlCommand<TDataModel, int> where TDataModel : DataModel<TDataModel>
{
    internal SqlDeleteCommand(
        TDataModel dataModel,
        DbConnection connection,
        DbTransaction transaction) :
        base(dataModel, [], [], connection, transaction, Constants.SqlDeleteTemplate)
    {
        RequireFilter = true;
    }
}
