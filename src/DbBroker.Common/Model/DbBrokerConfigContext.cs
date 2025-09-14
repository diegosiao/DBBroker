using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContext
{
    public string Namespace { get; set; }

    public string Name { get; set; }

    public SupportedDatabaseProviders? Provider { get; set; }

    public string ConnectionString { get; set; }

    /// <summary>
    /// The ConnectionString key on application settings if you prefer to use them instead of declaring the Connection String on a dbbroker.config.json file.
    /// <para>Can be overriden by ConnectionString value.</para>
    /// </summary>
    public string ConnectionStringKey { get; set; }

    /// <summary>
    /// Optional. Will override the default behavior of using the namespace as folder structure.
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

    /// <summary>
    /// Should DBBroker ignore database tables not described on '<see cref="Tables" />' collection?. The default value is 'false'.
    /// </summary>
    public bool IgnoreTablesNotListed { get; set; }

    /// <summary>
    /// Should DBBroker ignore database views not described on '<see cref="Views"/>' collection?. The default value is 'false'.
    /// </summary>
    public bool IgnoreViewsNotListed { get; set; }

    /// <summary>
    /// Remove all preexisting files from the output directory before syncing
    /// </summary>
    public bool ClearOutputDirectory { get; set; }

    public IEnumerable<DbBrokerConfigContextTable> Tables { get; set; } = [];

    public IEnumerable<DbBrokerConfigContextView> Views { get; set; } = [];
}