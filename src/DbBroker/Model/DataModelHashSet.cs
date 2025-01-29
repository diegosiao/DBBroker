using System.Collections.Generic;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

public class DataModelHashSet<TDataModel> : HashSet<TDataModel> where TDataModel : IDataModel
{
    public TDataModel DataModel { get; set; }

    public DataModelHashSet() : base(DataModelKeyComparer<TDataModel>.Instance)
    {
        
    }
}
