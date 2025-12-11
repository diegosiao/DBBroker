using DbBroker.Common.Model;

namespace DbBroker.Cli.Services.Providers.SqlServer;

internal class SqlServerMetadataProvider : MetadataProvider
{
    internal SqlServerMetadataProvider() : base(SqlServerConstants.SELECT_TABLES_COLUMNS, SqlServerConstants.SELECT_VIEWS_COLUMNS, SqlServerConstants.SELECT_KEYS)
    {
    }
    
    public override string GetTableFilterExpression(DbBrokerConfigContext context, bool checkAddReferencesConfig = false)
    {
        if (!context.IgnoreTablesNotListed || !context.Tables.Any())
        {
            return string.Empty;
        }

        return $"AND lower(t.name) IN ({string.Join(',', context.Tables.Select(x => $"'{x.Name?.ToLower()}'"))})";
    }

    public override string GetViewsFilterExpression(DbBrokerConfigContext context)
    {
        throw new NotImplementedException();
    }
}
