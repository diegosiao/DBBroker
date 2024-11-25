using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers.Oracle;

public class OracleExplicitKeySqlInsertTemplate : ISqlInsertTemplate
{
    public string SqlTemplate =>
        @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$)";

    public bool IncludeKeyColumn => true;

    public bool TryRetrieveKey => false;

    public Dictionary<string, string> Parameters => [];

    private static OracleExplicitKeySqlInsertTemplate _instance = new();

    public ISqlInsertTemplate Instance => _instance;

    public string ReplaceParameters(string sqlInsert) => sqlInsert;
}
