using System.Collections.Generic;
using System.Reflection;
using DBBroker.Model.Interfaces;

namespace DBBroker.Model;

public abstract class DataModelBase : IDataModel
{
    private static List<PropertyInfo> _properties;

    internal static List<PropertyInfo> Properties 
    { 
        get 
        {
            if (_properties == null)
            {
                _properties = new List<PropertyInfo>();
            }

            return _properties;
        } 
    }

    protected Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);

    internal List<PropertyInfo> GetCachedProperties() => Properties;

    // WIP
    // private string[] _isNotPristine = [8]; // number of properties
}
