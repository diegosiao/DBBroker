using System.Collections.Generic;
using System.Reflection;
using DbBroker.Common;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model;

/// <summary>
/// Mapping details for a Data Model type.
/// </summary>
public class DataModelMap
{
    /// <summary>
    /// The Data Model type this mapping applies to.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// The schema name for the table, if applicable.
    /// </summary>
    public string SchemaName { get; set; }

    /// <summary>
    /// The full table name including schema.
    /// </summary>
    public string TableFullName => $"{SchemaName}.{TableName}";

    /// <summary>
    /// The PropertyInfo for the key property of the Data Model.
    /// </summary>
    public PropertyInfo KeyProperty { get; set; }

    /// <summary>
    /// The Database provider or vendor for this Data Model and its context.
    /// </summary>
    public SupportedDatabaseProviders Provider { get; set; }

    /// <summary>
    /// Key: Database colum name; Value: mapping details
    /// </summary>
    public Dictionary<string, DataModelMapProperty> MappedProperties { get; set; } = [];

    /// <summary>
    /// Key: Property name; Value: mapping details
    /// </summary>
    public Dictionary<string, DataModelMapReference> MappedReferences { get; set; } = [];

    /// <summary>
    /// Key: Property name; Value: mapping details
    /// </summary>
    public Dictionary<string, DataModelMapCollectionReference> MappedCollections { get; set; } = [];

    /// <summary>
    /// The SQL Insert template for this Data Model type.
    /// </summary>
    public ISqlInsertTemplate SqlInsertTemplate { get; set; }
}
