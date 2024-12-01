namespace DbBroker;

public class Constants
{
    public const string SqlSelectTemplate = @"
SELECT $$COLUMNS$$ 
FROM $$TABLEFULLNAME$$ d0
$$JOINS$$
WHERE 1=1 $$FILTERS$$
$$ORDERBYCOLUMNS$$
$$OFFSETFETCH$$";

    public const string SqlUpdateTemplate = @"
UPDATE $$TABLEFULLNAME$$
SET $$COLUMNS$$
WHERE 1=1
$$FILTERS$$";

    public const string SqlDeleteTemplate = @"
DELETE $$TABLEFULLNAME$$
WHERE 1=1
$$FILTERS$$";
}
