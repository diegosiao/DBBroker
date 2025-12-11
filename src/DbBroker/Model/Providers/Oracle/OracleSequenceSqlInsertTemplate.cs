using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers.Oracle;

/// <summary>
/// SQL Insert template for Oracle databases when using a sequence to generate primary keys
/// </summary>
public class OracleSequenceSqlInsertTemplate : ISqlInsertTemplate
{
    /// <summary>
    /// SQL Insert template string
    /// </summary>
    public string SqlTemplate =>
@$"BEGIN
    INSERT INTO $$TABLEFULLNAME$$($$KEY_COLUMN$$, $$COLUMNS$$)
    VALUES ($$SEQUENCENAME$$.NEXTVAL, $$PARAMETERS$$);

    :pKey := $$SEQUENCENAME$$.CURRVAL;
END;";

    private static readonly Dictionary<string, string> _parameters = new()
    {
        { "SequenceName", "Name of the database sequence associated with this template and/or target table." }
    };

    /// <summary>
    /// Indicates whether the key column should be included in the insert statement
    /// </summary>
    public bool IncludeKeyColumn => false;

    /// <summary>
    /// Indicates whether the key can be retrieved after insertion
    /// </summary>
    public bool TryRetrieveKey => true;

    /// <summary>
    /// Name of the database sequence associated with this template and/or target table.
    /// </summary>
    public string SequenceName { get; private set; }

    /// <summary>
    /// Parameters dictionary
    /// </summary>
    public Dictionary<string, string> Parameters => _parameters;

    /// <summary>
    /// Singleton instance of the template
    /// </summary>
    public ISqlInsertTemplate Instance => null;

    /// <summary>
    /// Indicates whether the insert operation is compatible with upsert operations
    /// </summary>
    public bool UpsertCompatible => false;

    /// <summary>
    /// Replaces parameters in the SQL insert string
    /// </summary>
    /// <param name="sqlInsert"></param>
    /// <returns></returns>
    public string ReplaceParameters(string sqlInsert)
    {
        return sqlInsert.Replace($"$$SEQUENCENAME$$", SequenceName);
    }

    /// <summary>
    /// Constructor for the template
    /// </summary>
    public OracleSequenceSqlInsertTemplate() { }

    /// <summary>
    /// Constructor for the template with sequence name
    /// </summary>
    /// <param name="sequenceName"></param>
    public OracleSequenceSqlInsertTemplate(string sequenceName)
    {
        SequenceName = sequenceName;
    }
}
