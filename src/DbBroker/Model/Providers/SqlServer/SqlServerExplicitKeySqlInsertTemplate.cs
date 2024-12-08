using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers;

public class SqlServerExplicitKeySqlInsertTemplate : ISqlInsertTemplate
{
    public bool IncludeKeyColumn => true;

    public string SqlTemplate => @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$);";

    public bool TryRetrieveKey => false;

    public Dictionary<string, string> Parameters => [];

    private static SqlServerExplicitKeySqlInsertTemplate _instance = new();

    public ISqlInsertTemplate Instance => _instance;

    public bool UpsertCompatible => true;

    public string ReplaceParameters(string sqlInsert) => sqlInsert;
}
