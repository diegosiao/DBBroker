using System;
using DbBroker.Common.Model.Interfaces;

namespace DbBroker.Model.Providers;

public class SqlServerExplicitKeySqlInsertTemplate : ISqlInsertTemplate
{
    public bool IncludeKeyColumn => true;

    public string SqlTemplate => @$"
    INSERT INTO $$TABLEFULLNAME$$($$COLUMNS$$)
    VALUES ($$PARAMETERS$$);";

    public string KeyOutputParameterName => string.Empty;

    public bool TryRetrieveKey => false;
}
