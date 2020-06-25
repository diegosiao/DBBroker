using System.Collections.Generic;

namespace DBBroker.Engine
{
    /// <summary>
    /// This class represents the first result set of any SQL script executed by the static method <see cref="DBBrokerLive"/>.ExecCmdSQL.
    /// </summary>
    public class DBBrokerLiveRowCollection : List<DBBrokerLiveRow>
    {
        internal DBBrokerLiveRowCollection() { }

        internal DBBrokerLiveRowCollection(int capacity) : base(capacity) { }

        internal DBBrokerLiveRowCollection(IEnumerable<DBBrokerLiveRow> list) : base(list) { }
    }
}
