using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContext
{
    public string Namespace { get; set; }

    public SupportedDatabaseProviders Provider { get; set; }

    public string ConnectionString { get; set; }

    /// <summary>
    /// Optional. Will override the default behavior of using the namespace as folder structure (skipping the first namespace segment when multiple).
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// The fully qualified type of the default template implementation of <see cref="Interfaces.ISqlInsertTemplate"/>
    /// </summary>
    public string DefaultSqlInsertTemplateTypeFullName { get; set; }

    public IEnumerable<DbBrokerConfigContextTable> Tables { get; set; } = [];
}