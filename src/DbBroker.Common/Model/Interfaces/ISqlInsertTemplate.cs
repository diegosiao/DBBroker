using System.Collections.Generic;

namespace DbBroker.Common.Model.Interfaces;

public interface ISqlInsertTemplate
{
    /// <summary>
    /// This specifies if the Key and its value will be added to the final SQL INSERT. For example, for tables where the key is database generated this property will be set to false.
    /// </summary>
    bool IncludeKeyColumn { get; }

    /// <summary>
    /// The template should include:
    /// <para>$$TABLEFULLNAME$$ - The schema and table name</para>
    /// <para>$$COLUMNS$$ - The CSV list of columns</para>
    /// <para>$$PARAMETERS$$ - The CSV parameters names to be inserted</para>
    /// Optionally:
    /// <para>$$SEQUENCENAME$$ - The schema and sequence name associated</para>
    /// </summary>
    string SqlTemplate { get; }

    /// <summary>
    /// Specify if an attempt to retrieve the key value should be made after an INSERT. Explicit keys does not need this attempt.
    /// <para>The property <see cref="KeyOutputParameterName"/> will be used for that attempt if specified.</para>
    /// </summary>
    bool TryRetrieveKey { get; }

    /// <summary>
    /// Additional information required for the implementation of this template.
    /// <para>The key is the name of the parameter and the value is a description for the parameter.</para>
    /// <para>E.g.: { "SequenceName", "Name of the database sequence associated with this template and/or target table." }</para>
    /// </summary>
    public Dictionary<string, string> Parameters { get; }

    /// <summary>
    /// The instance value if the template can be shared with multiple Data Models, null if a new instance should be created for every reference.
    /// </summary>
    public ISqlInsertTemplate Instance { get; }

    public string ReplaceParameters(string sqlInsert);
}
