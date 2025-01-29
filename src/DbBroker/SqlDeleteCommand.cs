using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlDeleteCommand<TDataModel> : SqlCommand<TDataModel, int> where TDataModel : DataModel<TDataModel>
{
    internal SqlDeleteCommand(
        TDataModel dataModel, 
        DbConnection connection, 
        DbTransaction transaction) : 
        base(dataModel, [], [], connection, transaction, Constants.SqlDeleteTemplate) {}
}
