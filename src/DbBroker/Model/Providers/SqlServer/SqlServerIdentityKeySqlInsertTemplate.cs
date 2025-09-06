using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers;

/// <summary>
/// SQL Insert template for SQL Server databases when the primary key is an identity column
/// </summary>
public class SqlServerIdentityKeySqlInsertTemplate : ISqlInsertTemplate
{
    /// <summary>
    /// SQL Insert template string
    /// </summary>
    public string SqlTemplate =>
@$"INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
VALUES ($$PARAMETERS$$);

SELECT SCOPE_IDENTITY();";

    /// <summary>
    /// Indicates whether the key column should be included in the insert statement
    /// </summary>
    public bool IncludeKeyColumn => false;

    /// <summary>
    /// Indicates whether the key can be retrieved after insertion
    /// </summary>
    public bool TryRetrieveKey => true;

    /// <summary>
    /// Parameters dictionary (empty for this template)
    /// </summary>
    public Dictionary<string, string> Parameters => [];

    private static SqlServerIdentityKeySqlInsertTemplate _instance = new();

    /// <summary>
    /// Singleton instance of the template
    /// </summary>
    public ISqlInsertTemplate Instance => _instance;

    /// <summary>
    /// Indicates whether the insert operation is compatible with upsert operations
    /// </summary>
    public bool UpsertCompatible => false;

    /// <summary>
    /// Replaces parameters in the SQL insert string (no-op for this template)
    /// </summary>
    /// <param name="sqlInsert"></param>
    /// <returns></returns>
    public string ReplaceParameters(string sqlInsert) => sqlInsert;
}
