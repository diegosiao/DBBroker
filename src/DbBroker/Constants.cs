namespace DbBroker;

internal class Constants
{
    public const string SqlSelectTemplate = @"
SELECT $$COLUMNS$$ 
FROM $$TABLEFULLNAME$$ d0
$$JOINS$$WHERE 1=1 $$FILTERS$$
$$ORDERBYCOLUMNS$$
$$OFFSETFETCH$$";

    public const string SqlSelectCountTemplate = @"
SELECT COUNT(*)
FROM $$TABLEFULLNAME$$
WHERE 1=1 
$$FILTERS$$";

    public const string SqlSelectSumTemplate = @"
SELECT SUM($$COLUMNS$$)
FROM $$TABLEFULLNAME$$
WHERE 1=1 
$$FILTERS$$";

    public const string SqlSelectAvgTemplate = @"
SELECT AVG($$COLUMNS$$)
FROM $$TABLEFULLNAME$$
WHERE 1=1 
$$FILTERS$$";

    public const string SqlSelectMaxTemplate = @"
SELECT MAX($$COLUMNS$$)
FROM $$TABLEFULLNAME$$
WHERE 1=1 
$$FILTERS$$";

    public const string SqlSelectMinTemplate = @"
SELECT MIN($$COLUMNS$$)
FROM $$TABLEFULLNAME$$
WHERE 1=1 
$$FILTERS$$";

    public const string SqlUpdateTemplate = @"
UPDATE $$TABLEFULLNAME$$
SET $$COLUMNS$$
WHERE 1=1
$$FILTERS$$";

    public const string SqlDeleteTemplate = @"
DELETE FROM $$TABLEFULLNAME$$
WHERE 1=1
$$FILTERS$$";
}
