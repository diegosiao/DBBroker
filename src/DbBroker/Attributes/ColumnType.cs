using System;

namespace DbBroker.Attributes;

/// <summary>
/// Specifies the database type of a column.
/// </summary>
public class ColumnType : Attribute
{
    /// <summary>
    /// Gets the database type of the column.
    /// </summary>
    public object ProviderDbType { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnType"/> class.
    /// </summary>
    /// <param name="providerDbType"></param>
    public ColumnType(object providerDbType)
    {
        ProviderDbType = providerDbType;
    }
}
