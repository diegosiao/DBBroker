namespace DbBroker.Cli.Services.Providers.Oracle;

public class OracleConstants
{
    /// <summary>
    /// Replace $$TABLESFILTER$$
    /// </summary>
    internal const string SELECT_TABLES_COLUMNS = @"
    SELECT
        t.owner AS SchemaName,
        t.table_name AS TableName,
        c.column_name AS ColumnName,
        c.data_type AS DataType,
        c.data_length AS MaxLength,
        c.data_precision AS DataTypePrecision,
        c.data_scale AS DataTypeScale,
        CASE c.nullable WHEN 'N' THEN 0 ELSE 1 END AS IsNullable
    FROM
        all_tables t
    INNER JOIN
        all_tab_columns c ON t.table_name = c.table_name AND t.owner = c.owner
    WHERE 
        t.owner = user
    $$TABLESFILTER$$
    ORDER BY
        t.owner, t.table_name, c.column_id";

    /// <summary>
    /// Replace $$VIEWSFILTER$$
    /// </summary>
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
    $$VIEWSFILTER$$
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
    WHERE 1=1
    $$PRIMARYKEYS$$
    
    UNION ALL

    SELECT t.owner AS SchemaName,
        t.table_name AS TableName,
        fkc.column_name AS ColumnName,
        fkc.constraint_name AS ConstraintName,
        'ForeignKey' AS ConstraintType,
        rfkc.table_name AS ReferencedTable,
        rfkc.column_name AS ReferencedColumn 
    FROM    
        all_tables t
    INNER JOIN 
        all_constraints fk ON fk.table_name = t.table_name
    INNER JOIN 
        all_cons_columns fkc ON fk.constraint_name = fkc.constraint_name AND fk.owner = fkc.owner
    INNER JOIN
        all_cons_columns rfkc ON rfkc.constraint_name = fk.r_constraint_name
    WHERE 
        t.owner = user
    AND 
        fk.constraint_type = 'R'
    $$FOREIGNKEYS$$
    ORDER BY
        SchemaName, TableName, ColumnName
    ";

}
