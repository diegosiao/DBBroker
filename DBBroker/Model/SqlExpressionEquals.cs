namespace DbBroker.Model;

public class SqlEquals : SqlExpression
{
    private SqlEquals(object value) : base(SqlOperator.Equals)
    {
        Parameters = [value];
    }

    public static SqlEquals To(object value)
    {
        return new SqlEquals(value);
    }

    public override string RenderSql(string columnName, int index)
    {
        return $"{columnName} = @{columnName}{index}";
    }
}
