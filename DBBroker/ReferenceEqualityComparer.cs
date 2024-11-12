using System.Collections.Generic;

namespace DbBroker;

public class ReferenceEqualityComparer1 : IEqualityComparer<object>
{ 
    public new bool Equals(object x, object y)
    {
        return ReferenceEquals(x, y);
    }

    public int GetHashCode(object obj)
    {
        return System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
    }
}
