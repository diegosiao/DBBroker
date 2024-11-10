using System;

namespace DbBroker;

public class Constants
{
    public const string SqlUpdateTemplate = @"
    UPDATE $$TABLEFULLNAME$$
    SET $$COLUMNS$$
    WHERE 1=1
    $$FILTERS$$;";
}
