using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContext
{
    public string Namespace { get; set; }

    public SupportedDatabaseProviders Provider { get; set; }

    public string ConnectionString { get; set; }

    /// <summary>
    /// The ConnectionString key on application settings if you prefer to use them instead of declaring the Connection String on a dbbroker.config.json file.
    /// <para>Can be overriden by ConnectionString value.</para>
    /// </summary>
    public string ConnectionStringKey { get; set; }

    /// <summary>
    /// Optional. Will override the default behavior of using the namespace as folder structure (skipping the first namespace segment when multiple).
    /// </summary>
    public string OutputDirectory { get; set; }

    /// <summary>
    /// The fully qualified type of the default template implementation of <see cref="Interfaces.ISqlInsertTemplate"/>
    /// <para>You can override this value on Table level using the 'SqlInsertTemplateTypeFullName' value.</para>
    /// </summary>
    public string DefaultSqlInsertTemplateTypeFullName { get; set; }

    public IEnumerable<DbBrokerConfigContextTable> Tables { get; set; } = [];
}