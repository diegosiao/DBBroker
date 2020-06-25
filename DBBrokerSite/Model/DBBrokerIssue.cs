using DBBroker.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Model
{
    [DBMappedClass(Table ="Issues", PrimaryKey ="IdIssue")]
    public class DBBrokerIssue : BusinessObject
    {        
        public DBBrokerUser User { get; set; }

        public Status Status { get; set; }

        public Nature Nature { get; set; }

        public string Language { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Response { get; set; }

        public string WorkAround { get; set; }

        [DBReadOnly(DBDefaultValue ="GETDATE()")]
        public DateTime Creation { get; set; }

        public DateTime? Resolution { get; set; }

        public DBBrokerIssue()
        {
            User = new DBBrokerUser();
            Status = new Status() { Id = 1, Description = "Open " };
        }
    }
}