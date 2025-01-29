using System.Collections;
using System.Collections.Generic;
using DbBroker.Model.Interfaces;

namespace DbBroker;

public class DataModelKeyComparer : IEqualityComparer<object>
{
    private static readonly IEqualityComparer structuralComparer = StructuralComparisons.StructuralEqualityComparer;

    public new bool Equals(object x, object y)
    {
        if (x is byte[] bx && y is byte[] by)
        {
            return structuralComparer.Equals(bx, by);
        }

        return EqualityComparer<object>.Default.Equals(x, y);
    }

    public int GetHashCode(object obj)
    {
        if (obj is byte[] b)
        {
            return structuralComparer.GetHashCode(b);
        }
        return EqualityComparer<object>.Default.GetHashCode(obj);
    }
}


public class DataModelKeyComparer<TDataModel> : IEqualityComparer<TDataModel> where TDataModel : IDataModel
{
    public static readonly DataModelKeyComparer<TDataModel> Instance = new();

    private DataModelKeyComparer() { }

    public bool Equals(TDataModel x, TDataModel y)
    {
        if (!x.DataModelMap.TableFullName.Equals(y.DataModelMap.TableFullName) ||
            x.DataModelMap.KeyProperty == null || y.DataModelMap.KeyProperty == null)
        {
            return false;
        }

        return x.GetHashCode().Equals(y.GetHashCode());
    }

    public int GetHashCode(TDataModel obj)
    {
        var hashCode = obj.DataModelMap.KeyProperty.GetValue(obj)?.GetHashCode() ?? -1;
        return hashCode;
    }
}
