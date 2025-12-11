using System.Collections.Generic;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

/// <summary>
/// A HashSet that uses DataModelKeyComparer for equality comparison and contains a reference to the DataModel type.
/// </summary>
/// <typeparam name="TDataModel"></typeparam>
public class DataModelHashSet<TDataModel> : HashSet<TDataModel> where TDataModel : IDataModel
{
    /// <summary>
    /// The DataModel type associated with this HashSet.
    /// </summary>
    public TDataModel DataModel { get; set; }

    /// <summary>
    /// Initializes a new instance of the DataModelHashSet class that is empty and uses DataModelKeyComparer for equality comparison.
    /// </summary>
    public DataModelHashSet() : base(DataModelKeyComparer<TDataModel>.Instance)
    {

    }
}
