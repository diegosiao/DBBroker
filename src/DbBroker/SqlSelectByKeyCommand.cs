using System;
using System.Collections.Generic;
using System.Data.Common;
using DbBroker.Model;

namespace DbBroker;

public class SqlSelectByKeyCommand<TDataModel> : SqlCommand<TDataModel, TDataModel> where TDataModel : DataModel<TDataModel>
{
    public SqlSelectByKeyCommand(
        TDataModel dataModel,
        DbConnection connection, 
        IEnumerable<DataModelMapProperty> columns,
        DbParameter primaryKey,
        int depth = 0,
        DbTransaction transaction = null) : base(dataModel, columns, [ primaryKey ], connection, transaction, Constants.SqlSelectCountTemplate)
    {
    }

    public override TDataModel Execute(int commandTimeout = 0)
    {
        throw new NotImplementedException();
    }
}
