using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers.Postgres;

/// <summary>
/// SQL Insert template for Postgres databases when the primary key is explicitly provided
/// </summary>
public class PostgresExplicitKeySqlInsertTemplate : ISqlInsertTemplate
{
    /// <summary>
    /// SQL Insert template string
    /// </summary>
    public string SqlTemplate =>
        @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$)";

    /// <summary>
    /// Indicates whether the key column should be included in the insert statement
    /// </summary>
    public bool IncludeKeyColumn => true;

    /// <summary>
    /// Indicates whether the key can be retrieved after insertion
    /// </summary>
    public bool TryRetrieveKey => false;

    /// <summary>
    /// Parameters dictionary (empty for this template)
    /// </summary>
    public Dictionary<string, string> Parameters => [];

    private static PostgresExplicitKeySqlInsertTemplate _instance = new();

    /// <summary>
    /// Singleton instance of the template
    /// </summary>
    public ISqlInsertTemplate Instance => _instance;

    /// <summary>
    /// Indicates whether the insert operation is compatible with upsert operations
    /// </summary>
    public bool UpsertCompatible => true;

    /// <summary>
    /// Replaces parameters in the SQL insert string (no-op for this template)
    /// </summary>
    /// <param name="sqlInsert"></param>
    /// <returns></returns>
    public string ReplaceParameters(string sqlInsert) => sqlInsert;
}
