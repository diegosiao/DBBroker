using System;
using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContextTable
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string PrimaryKeyColumn { get; set; }

    public string SqlInsertTemplateTypeFullName { get; set; }

    public string SqlInsertTemplate { get; set; }

    public Dictionary<string, object> SqlInsertTemplateArguments { get; set; }

    // TODO the logic to deal with read only tables is missing
    public bool ReadOnly { get; set; }

    public bool AddReferences { get; set; } = true;

    public IEnumerable<DbBrokerConfigContextColumn> Columns { get; set; }
}
