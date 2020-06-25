using System;

namespace DBBroker.Mapping
{
    /// <summary>
    /// Informs to DBBroker that this property is not persisted and should be ignored.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBTransient : Map
    {
    }
}
