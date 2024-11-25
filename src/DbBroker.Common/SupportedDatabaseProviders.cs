namespace DbBroker.Common;

/// <summary>
/// Database providers supported by DBBroker CLI and library.
/// </summary>
public enum SupportedDatabaseProviders
{
    /// <summary>
    /// Microsoft SQL Server
    /// <para>Required Client Library: System.Data.SqlClient</para>
    /// <para>Tested against:</para>
    /// <para>- 2022</para>
    /// </summary>
    SqlServer,

    /// <summary>
    /// Oracle
    /// <para>Required Client Library: Oracle.ManagedDataAccess.Core</para>
    /// <para>Tested against:</para>
    /// <para>- 11g</para>
    /// </summary>
    Oracle,

    // Roadmap...
    // Postgres,
    // MySQl
}
