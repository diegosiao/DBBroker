using System.Collections.Generic;
using DbBroker.Common;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model;

public class DataModelMap
{
    public string TableName { get; set; }

    public string SchemaName { get; set; }

    public string TableFullName => $"{SchemaName}.{TableName}";

    public SupportedDatabaseProviders Provider { get; set; }
    
    /// <summary>
    /// Key: Database colum name; Value: mapping details
    /// </summary>
    public Dictionary<string, DataModelMapProperty> MappedProperties { get; set; } = [];

    public Dictionary<string, DataModelMapReference> MappedReferences { get; set; } = [];

    public ISqlInsertTemplate SqlInsertTemplate { get; set; }
}
