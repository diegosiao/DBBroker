using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContext
{
    public string Namespace { get; set; }

    public string Name { get; set; }

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

    /// <summary>
    /// The name prefix that Entity Data Models classes will receive when generated. The default value is empty.
    /// </summary>
    public string ModelsPrefix { get; set; }

    /// <summary>
    /// The name sufix that Entity Data Models classes will receive when generated. The default value is 'DataModel'.
    /// </summary>
    public string ModelsSufix { get; set; } = "DataModel";

    public IEnumerable<DbBrokerConfigContextTable> Tables { get; set; } = [];
}