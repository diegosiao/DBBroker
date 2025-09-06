namespace DbBroker.Common;

/// <summary>
/// Database providers supported by DBBroker CLI and library.
/// </summary>
public enum SupportedDatabaseProviders
{
    /// <summary>
    /// Microsoft SQL Server
    /// <para>Required Client Library: Microsoft.Data.SqlClient</para>
    /// <para>Tested against:</para>
    /// <para>- 2019</para>
    /// <para>- 2022</para>
    /// </summary>
    SqlServer,

    /// <summary>
    /// Oracle
    /// <para>Required Client Library: <a href='https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core'>Oracle.ManagedDataAccess.Core</a></para>
    /// <para>Tested against:</para>
    /// <para>- 11g</para>
    /// <para>- 12c</para>
    /// <para>- 19c</para>
    /// <para>- 21c</para>
    /// </summary>
    Oracle,

    // Roadmap...
    // Postgres,
    // MySQl
}
