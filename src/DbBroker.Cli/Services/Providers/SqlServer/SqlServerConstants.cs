namespace DbBroker.Cli.Services.Providers.SqlServer;

public class SqlServerConstants
{
    internal const string SELECT_TABLES_COLUMNS = @"
    SELECT
        s.name AS SchemaName,
        t.name AS TableName,
        c.name AS ColumnName,
        ty.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM
        sys.tables AS t
    INNER JOIN
        sys.schemas AS s ON t.schema_id = s.schema_id
    INNER JOIN
        sys.columns AS c ON t.object_id = c.object_id
    INNER JOIN
        sys.types AS ty ON c.user_type_id = ty.user_type_id
    ORDER BY
        s.name, t.name, c.column_id;";

    internal const string SELECT_VIEWS_COLUMNS = @"
    SELECT
        s.name AS SchemaName,
        v.name AS ViewName,
        c.name AS ColumnName,
        ty.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM
        sys.views AS v
    INNER JOIN
        sys.schemas AS s ON v.schema_id = s.schema_id
    INNER JOIN
        sys.columns AS c ON v.object_id = c.object_id
    INNER JOIN
        sys.types AS ty ON c.user_type_id = ty.user_type_id
    ORDER BY
        s.name, v.name, c.column_id;";

    internal const string SELECT_KEYS = @"
    SELECT
        s.name AS SchemaName,
        t.name AS TableName,
        c.name AS ColumnName,
        k.name AS ConstraintName,
        'PrimaryKey' AS ConstraintType,
        NULL AS ReferencedTable,
        NULL AS ReferencedColumn
    FROM
        sys.tables t
    JOIN
        sys.schemas s ON t.schema_id = s.schema_id
    JOIN
        sys.key_constraints k ON t.object_id = k.parent_object_id
    JOIN
        sys.index_columns ic ON k.unique_index_id = ic.index_id AND k.parent_object_id = ic.object_id
    JOIN
        sys.columns c ON ic.column_id = c.column_id AND c.object_id = t.object_id
    WHERE
        k.type = 'PK'

    UNION ALL

    SELECT
        s.name AS SchemaName,
        tp.name AS TableName,
        cp.name AS ColumnName,
        fk.name AS ConstraintName,
        'ForeignKey' AS ConstraintType,
        tr.name AS ReferencedTable,
        cr.name AS ReferencedColumn
    FROM
        sys.foreign_keys fk
    JOIN
        sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    JOIN
        sys.tables tp ON fkc.parent_object_id = tp.object_id
    JOIN
        sys.columns cp ON fkc.parent_column_id = cp.column_id AND fkc.parent_object_id = cp.object_id
    JOIN
        sys.schemas s ON tp.schema_id = s.schema_id
    JOIN
        sys.tables tr ON fkc.referenced_object_id = tr.object_id
    JOIN
        sys.columns cr ON fkc.referenced_column_id = cr.column_id AND fkc.referenced_object_id = cr.object_id
    ORDER BY
        SchemaName, TableName, ColumnName;
    ";

}
