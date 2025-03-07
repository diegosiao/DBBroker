namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleConstants
{
    internal const string SELECT_TABLES_COLUMNS = @"
    SELECT
        t.owner AS SchemaName,
        t.table_name AS TableName,
        c.column_name AS ColumnName,
        c.data_type AS DataType,
        c.data_length AS MaxLength,
        CASE c.nullable WHEN 'N' THEN 0 ELSE 1 END AS IsNullable
    FROM
        all_tables t
    INNER JOIN
        all_tab_columns c ON t.table_name = c.table_name AND t.owner = c.owner
    WHERE t.owner = user
    ORDER BY
        t.owner, t.table_name, c.column_id";

    internal const string SELECT_VIEWS_COLUMNS = @"
    SELECT
        v.owner AS SchemaName,
        v.view_name AS ViewName,
        c.column_name AS ColumnName,
        c.data_type AS DataType,
        c.data_length AS MaxLength,
        CASE c.nullable WHEN 'N' THEN 0 ELSE 1 END AS IsNullable
    FROM
        all_views v
    INNER JOIN
        all_tab_columns c ON v.view_name = c.table_name AND v.owner = c.owner
    WHERE
        v.owner = user
    ORDER BY
        v.owner, v.view_name, c.column_id";

    internal const string SELECT_KEYS = @"
    SELECT
        u.username AS SchemaName,
        t.table_name AS TableName,
        c.column_name AS ColumnName,
        cc.constraint_name AS ConstraintName,
        'PrimaryKey' AS ConstraintType,
        NULL AS ReferencedTable,
        NULL AS ReferencedColumn
    FROM
        all_tables t
    INNER JOIN
        all_tab_columns c ON t.table_name = c.table_name AND t.owner = c.owner AND t.owner = user
    INNER JOIN
        all_cons_columns cc ON c.table_name = cc.table_name AND c.column_name = cc.column_name AND c.owner = cc.owner
    INNER JOIN
        all_constraints con ON cc.constraint_name = con.constraint_name AND cc.owner = con.owner AND con.CONSTRAINT_TYPE = 'P'
    INNER JOIN
        all_users u ON t.owner = u.username

    UNION ALL

    SELECT
        u.username AS SchemaName,
        tp.table_name AS TableName,
        cp.column_name AS ColumnName,
        fk.constraint_name AS ConstraintName,
        'ForeignKey' AS ConstraintType,
        tr.table_name AS ReferencedTable,
        cr.column_name AS ReferencedColumn
    FROM
        all_constraints fk
    INNER JOIN
        all_cons_columns fkc ON fk.constraint_name = fkc.constraint_name AND fk.owner = fkc.owner
    INNER JOIN
        all_tables tp ON fkc.table_name = tp.table_name AND fkc.owner = tp.owner AND tp.owner = user
    INNER JOIN
        all_tab_columns cp ON fkc.table_name = cp.table_name AND fkc.column_name = cp.column_name AND fkc.owner = cp.owner
    INNER JOIN
        all_users u ON tp.owner = u.username
    INNER JOIN
        all_constraints pk ON fk.r_constraint_name = pk.constraint_name AND fk.r_owner = pk.owner
    INNER JOIN
        all_cons_columns pkk ON pk.constraint_name = pkk.constraint_name AND pk.owner = pkk.owner
    INNER JOIN
        all_tables tr ON pkk.table_name = tr.table_name AND pkk.owner = tr.owner
    INNER JOIN
        all_tab_columns cr ON pkk.table_name = cr.table_name AND pkk.column_name = cr.column_name AND pkk.owner = cr.owner
    WHERE
        fk.constraint_type = 'R'
    ORDER BY
        SchemaName, TableName, ColumnName
    ";

}
