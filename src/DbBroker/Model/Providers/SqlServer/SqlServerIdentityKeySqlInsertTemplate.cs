using System.Collections.Generic;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers;

public class SqlServerIdentityKeySqlInsertTemplate : ISqlInsertTemplate
{
    public string SqlTemplate =>
@$"INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
VALUES ($$PARAMETERS$$);

SELECT SCOPE_IDENTITY();";

    public bool IncludeKeyColumn => false;

    public bool TryRetrieveKey => true;

    public Dictionary<string, string> Parameters => [];

    private static SqlServerIdentityKeySqlInsertTemplate _instance = new();
    
    public ISqlInsertTemplate Instance => _instance;

    public bool UpsertCompatible => false;

    public string ReplaceParameters(string sqlInsert) => sqlInsert;
}
