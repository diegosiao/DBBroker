namespace DbBroker.Model;

/// <summary>
/// Enumeration of SQL Operators
/// </summary>
public enum SqlOperator
{
    /// <summary>
    /// Equals SQL operator
    /// </summary>
    Equals,

    /// <summary>
    /// Not equals SQL operator
    /// </summary>
    NotEquals,

    /// <summary>
    /// Less than or less than or equal to SQL operator
    /// </summary>
    LessThan,

    /// <summary>
    /// Less than or equal to SQL operator
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Greater than or greater than or equal to SQL operator
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Greater than or equal to SQL operator
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Between SQL operator
    /// </summary>
    Between,

    /// <summary>
    /// Is SQL operator
    /// </summary>
    Is,

    /// <summary>
    /// In SQL operator
    /// </summary>
    In,

    /// <summary>
    /// Like SQL operator
    /// </summary>
    Like
}
