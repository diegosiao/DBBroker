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

    public IEnumerable<DbBrokerConfigContextColumn> Columns { get; set; }
}
