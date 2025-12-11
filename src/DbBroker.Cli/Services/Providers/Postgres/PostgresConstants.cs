using System;

namespace DbBroker.Cli.Services.Providers.Postgres;

public class PostgresConstants
{
/// <summary>
    /// Replace $$TABLESFILTER$$
    /// </summary>
    internal const string SELECT_TABLES_COLUMNS = @"
    SELECT
        table_schema AS SchemaName,
        table_name AS TableName,
        column_name AS ColumnName,
        data_type AS DataType,
        character_maximum_length AS MaxLength,
        numeric_precision AS DataTypePrecision,
        numeric_scale AS DataTypeScale,
        CASE is_nullable WHEN 'NO' THEN 0 ELSE 1 END AS IsNullable
    FROM
        information_schema.columns
    WHERE
        table_schema = current_schema()
        --$$TABLESFILTER$$
    ORDER BY
        table_schema, table_name, ordinal_position;";

    /// <summary>
    /// Replace $$VIEWSFILTER$$
    /// </summary>
    internal const string SELECT_VIEWS_COLUMNS = @"
    SELECT
        c.table_schema AS SchemaName,
        c.table_name AS ViewName,
        c.column_name AS ColumnName,
        c.data_type AS DataType,
        c.character_maximum_length AS MaxLength,
        CASE c.is_nullable WHEN 'NO' THEN 0 ELSE 1 END AS IsNullable
    FROM
        information_schema.columns c
    INNER JOIN
        information_schema.views v
        ON c.table_schema = v.table_schema AND c.table_name = v.table_name
    WHERE
        c.table_schema = current_schema()
        $$VIEWSFILTER$$
    ORDER BY
        c.table_schema, c.table_name, c.ordinal_position;";

    internal const string SELECT_KEYS = @"
    -- Primary Keys
    SELECT
        n.nspname AS SchemaName,
        t.relname AS TableName,
        a.attname AS ColumnName,
        con.conname AS ConstraintName,
        'PrimaryKey' AS ConstraintType,
        NULL AS ReferencedTable,
        NULL AS ReferencedColumn
    FROM
        pg_constraint con
    JOIN
        pg_class t ON con.conrelid = t.oid
    JOIN
        pg_namespace n ON t.relnamespace = n.oid
    JOIN
        pg_attribute a ON a.attrelid = t.oid AND a.attnum = ANY(con.conkey)
    WHERE
        con.contype = 'p'

    UNION ALL

    -- Foreign Keys
    SELECT
        n.nspname AS SchemaName,
        t.relname AS TableName,
        a.attname AS ColumnName,
        con.conname AS ConstraintName,
        'ForeignKey' AS ConstraintType,
        rt.relname AS ReferencedTable,
        ra.attname AS ReferencedColumn
    FROM
        pg_constraint con
    JOIN
        pg_class t ON con.conrelid = t.oid
    JOIN
        pg_namespace n ON t.relnamespace = n.oid
    JOIN
        pg_attribute a ON a.attrelid = t.oid AND a.attnum = ANY(con.conkey)
    JOIN
        pg_class rt ON con.confrelid = rt.oid
    JOIN
        pg_attribute ra ON ra.attrelid = rt.oid AND ra.attnum = ANY(con.confkey)
    WHERE
        con.contype = 'f'
    ORDER BY
        SchemaName, TableName, ColumnName;
    ";

}
