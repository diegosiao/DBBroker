using DBBroker.Engine;
using DBBrokerSite.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBBrokerSite.Persistence
{
    public class DB_DBBrokerIssue : DBBroker<DBBrokerIssue>
    {
        public static DBBrokerIssue GetByToken(string user_token)
        {
            List<DBBrokerIssue> result = 
                ExecCmdSQL(
                    "SELECT * FROM Issues WHERE EXISTS(SELECT * FROM Users WHERE TokenCode = @Token) ORDER BY IdIssue DESC; "
                    , new List<DbParameter>() { new SqlParameter("@Token", user_token) });

            return result.Count == 0 ? new DBBrokerIssue() : result[0];
        }

        internal static List<DBBrokerIssue> GetActiveBugs()
        {
            return ExecCmdSQL("SELECT * FROM Issues WHERE IdStatus IN (2,3) AND IdNature = 2;");
        }
    }
}