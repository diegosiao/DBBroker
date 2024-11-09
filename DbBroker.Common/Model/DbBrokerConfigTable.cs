using System;
using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfigContextTable
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string PrimaryKeyColumn { get; set; }

    public IEnumerable<DbBrokerConfigContextColumn> Columns { get; set; }
}
