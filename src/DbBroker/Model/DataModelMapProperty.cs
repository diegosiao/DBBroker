using System.Reflection;

namespace DbBroker.Model;

public class DataModelMapProperty
{
    public string ColumnName { get; set; }

    public string PropertyName => PropertyInfo?.Name;

    public bool IsKey { get; set; }

    public int Index { get; set; }

    public object ProviderDbType { get; set; }

    public PropertyInfo PropertyInfo { get; set; }
}
