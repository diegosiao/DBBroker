using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers.Oracle;

public class OracleExplicitKeySqlInsertTemplate : ISqlInsertTemplate
{
    public string SqlTemplate =>
        @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$)";

    public string KeyOutputParameterName => string.Empty;

    public bool IncludeKeyColumn => true;

    public bool TryRetrieveKey => false;
}
