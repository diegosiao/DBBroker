using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers.Oracle;

public class OracleSequenceSqlInsertTemplate : ISqlInsertTemplate
{
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

    public bool IncludeKeyColumn => false;

    public bool TryRetrieveKey => true;

    public string SequenceName { get; private set; }

    public Dictionary<string, string> Parameters => _parameters;

    public ISqlInsertTemplate Instance => null;

    public bool UpsertCompatible => false;

    public string ReplaceParameters(string sqlInsert)
    {
        return sqlInsert.Replace($"$$SEQUENCENAME$$", SequenceName);
    }

    public OracleSequenceSqlInsertTemplate() { }

    public OracleSequenceSqlInsertTemplate(string sequenceName)
    {
        SequenceName = sequenceName;
    }
}
