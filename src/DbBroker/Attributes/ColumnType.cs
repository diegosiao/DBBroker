using System;

namespace DbBroker.Attributes;

public class ColumnType : Attribute
{
    public object ProviderDbType { get; private set; }

    public ColumnType(object providerDbType)
    {
        ProviderDbType = providerDbType;
    }
}
