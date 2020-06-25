using DBBroker.Engine;
using DBBroker.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Model
{
    [DBMappedClass(Table ="AppLog", PrimaryKey ="IdLog")]
    public class AppLog : BusinessObject
    {
        public string Message { get; set; }
        public string Stack { get; set; }

        [DBReadOnly(DBDefaultValue ="GETDATE()")]
        public DateTime Creation { get; set; }

        public bool IsChecked { get; set; }

        public static void Log(Exception ex)
        {
            try
            {
                AppLog log = new AppLog();
                log.Message = ex.Message;
                log.Stack = ex.StackTrace;

                DBBroker<AppLog>.Save(log);
            }
            catch { }
        }
    }
}