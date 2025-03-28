using System;
using System.Collections.Generic;

namespace DbBroker.Common.Model;

public class DbBrokerConfig
{
    public DbBrokerConfigDatabase Database { get; set; }
    
    public IEnumerable<DbBrokerConfigContext> Contexts { get; set; }

    public DateTime? LastSynced { get; set; }
}