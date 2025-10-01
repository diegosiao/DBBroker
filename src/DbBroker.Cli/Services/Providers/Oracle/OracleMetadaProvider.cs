using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers.Oracle;

internal class OracleMetadaProvider : MetadataProvider
{
    internal OracleMetadaProvider()
        : base(
            OracleConstants.SELECT_TABLES_COLUMNS,
            OracleConstants.SELECT_VIEWS_COLUMNS,
            OracleConstants.SELECT_KEYS)
    {
    }

    public override string GetTableFilterExpression(DbBrokerConfigContext context, bool checkAddReferencesConfig = false)
    {
        if (!context.IgnoreTablesNotListed || !context.Tables.Any())
        {
            return string.Empty;
        }

        if (checkAddReferencesConfig)
        {
            // That will define/restrict the FOREIGN KEYS select 
            var tables = context.Tables.Where(t => context.Tables.Any(x => x.AddReferences && x.Name == t.Name));

            return tables.Any() ? 
                $"AND lower(t.table_name) IN ({string.Join(',', tables.Select(x => $"'{x.Name?.ToLower()}'"))})" :
                "AND 1 <> 1";
        }

        return $"AND lower(t.table_name) IN ({string.Join(',', context.Tables.Select(x => $"'{x.Name?.ToLower()}'"))})";
    }

    public override string GetViewsFilterExpression(DbBrokerConfigContext context)
    {
        if (!context.IgnoreViewsNotListed || !context.Tables.Any())
        {
            return string.Empty;
        }

        return $"AND lower(v.view_name) IN ({string.Join(',', context.Views.Select(x => $"'{x.Name?.ToLower()}'"))})";
    }
}
