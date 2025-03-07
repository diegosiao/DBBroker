using DbBroker.Common.Model;
using DbBroker.Model.Interfaces;

namespace DbBroker.Model;

public abstract class ViewDataModel<T> : DataModel<T> where T : class, IViewDataModel
{
    internal static DbBrokerConfigContextView DbBrokerConfigContextView { get; set; }
}
