using DbBroker.Common.Model;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker;

/// <summary>
/// Base class for view data models.
/// </summary>
/// <typeparam name="T">DBBroker generated Data Model</typeparam>
public abstract class ViewDataModel<T> : DataModel<T> where T : class, IViewDataModel
{
    internal static DbBrokerConfigContextView DbBrokerConfigContextView { get; set; }
}
