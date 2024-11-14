using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DbBroker.Model.Interfaces;

namespace DbBroker;

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

    public int GetHashCode([DisallowNull] TDataModel obj)
    {
        var hashCode = obj.DataModelMap.KeyProperty.GetValue(obj)?.GetHashCode() ?? -1;
        return hashCode;
    }
}
