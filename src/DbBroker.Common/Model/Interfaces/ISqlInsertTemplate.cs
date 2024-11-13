namespace DbBroker.Common.Model.Interfaces;

public interface ISqlInsertTemplate
{
    /// <summary>
    /// This specifies if the Key and its value will be added to the final SQL INSERT. For example, for tables where the key is database generated this property will be set to false.
    /// </summary>
    bool IncludeKeyColumn { get; }

    /// <summary>
    /// <para>$$TABLEFULLNAME$$ - The schema and table name</para>
    /// <para>$$COLUMNS$$ - The list of columns</para>
    /// <para>$$PARAMETERS$$ - The parameters to be inserted</para>
    /// </summary>
    string SqlTemplate { get; }

    /// <summary>
    /// Inform this value to replace the default behavior of retrieving the key from a scalar query.
    /// After execution, an attempt to load the value from an output parameter with this specified name will be made. 
    /// </summary>
    string KeyOutputParameterName { get; }

    /// <summary>
    /// Specify if an attempt to retrieve the key value should be made after an INSERT. Explicit keys does not need this attempt.
    /// </summary>
    bool TryRetrieveKey { get; }
}
