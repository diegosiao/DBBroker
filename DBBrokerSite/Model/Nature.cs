using DBBroker.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Model
{
    [DBMappedClass(Table ="Natures", PrimaryKey ="IdNature")]
    public class Nature : BusinessObject
    {
        public string Description { get; set; }
    }
}