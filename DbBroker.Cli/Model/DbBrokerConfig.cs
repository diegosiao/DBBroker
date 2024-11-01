namespace DbBroker.Cli.Model;

public class DbBrokerConfig
{
    public IEnumerable<DbBrokerConfigDatabase>? Databases { get; set; }

    public DateTime? LastSynced { get; set; }
}

public class DbBrokerConfigDatabase
{
    public required string Namespace { get; set; }

    public SupportedDatabaseProviders Vendor { get; set; } = SupportedDatabaseProviders.SqlServer;

    public required string ConnectionString { get; set; }

    public IEnumerable<DbBrokerConfigDatabaseTable>? Tables { get; set; }
}

public class DbBrokerConfigDatabaseTable
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? PrimaryKeyColumn { get; set; }

    public required IEnumerable<DbBrokerConfigDatabaseColumn> Columns { get; set; }
}

public class DbBrokerConfigDatabaseColumn
{
    /*
    SELECT
    t.table_name,
    c.column_name,
    c.data_type,
    c.data_length,
    c.nullable,
    c.data_default,
    o.last_ddl_time,
    t.owner
FROM
    all_tables t
JOIN
    all_tab_columns c ON t.table_name = c.table_name
--JOIN
--    all_objects o ON t.table_name = o.object_name AND o.object_type = 'TABLE'
--WHERE t.owner = 'YOUR_SCHEMA'
ORDER BY
    t.table_name, c.column_id;
    */
    public Guid Id { get; set; }

    public Guid TableId { get; set; }

    public required string SchemaName { get; set; }

    public required string TableName { get; set; }

    public required string ColumnName { get; set; }

    public required string DataType { get; set; }

    public string? MaxLength { get; set; }

    public bool IsNullable { get; set; }
}