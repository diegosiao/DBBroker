using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers.Postgres;

internal class PostgresMetadataProvider : MetadataProvider
{
    internal PostgresMetadataProvider() : base(PostgresConstants.SELECT_TABLES_COLUMNS, PostgresConstants.SELECT_VIEWS_COLUMNS, PostgresConstants.SELECT_KEYS)
    {
    }
    
    public override string GetTableFilterExpression(DbBrokerConfigContext context, bool checkAddReferencesConfig = false)
    {
        if (!context.IgnoreTablesNotListed || !context.Tables.Any())
        {
            return string.Empty;
        }

        return $"AND lower(table_name) IN ({string.Join(',', context.Tables.Select(x => $"'{x.Name?.ToLower()}'"))})";
    }

    public override string GetViewsFilterExpression(DbBrokerConfigContext context)
    {
        if (!context.IgnoreViewsNotListed || !context.Tables.Any())
        {
            return string.Empty;
        }

        return $"AND lower(v.table_name) IN ({string.Join(',', context.Views.Select(x => $"'{x.Name?.ToLower()}'"))})";
    }
}
