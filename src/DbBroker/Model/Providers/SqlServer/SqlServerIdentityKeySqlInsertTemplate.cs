using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers;

public class SqlServerIdentityKeySqlInsertTemplate : ISqlInsertTemplate
{
    public string SqlTemplate => 
    @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$);
    
    SELECT SCOPE_IDENTITY();";

    public string KeyOutputParameterName => string.Empty;

    public bool IncludeKeyColumn => false;

    public bool TryRetrieveKey => true;
}
