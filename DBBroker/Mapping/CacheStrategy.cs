using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBBroker.Mapping
{
    /// <summary>
    /// Internal while it is just a vague idea. No implementation has been made.
    /// </summary>
    internal enum CacheStrategy
    {
        /// <summary>
        /// Every request will reach the database
        /// </summary>
        None,
        /// <summary>
        /// That means one load for application session when the first request occurs.
        /// <para></para>
        /// </summary>
        Static,
        /// <summary>
        /// Will manage for you based on the call of ExecCmdSql, meaning that after
        /// every call to 'ExecCmdSql' of this class context will trigger a request 
        /// for refreshing the cache.
        /// <para>This strategy only consider the instance of the executing application, 
        /// which can lead to outdated data due to changes made by other instances.</para>
        /// </summary>
        Automatic
    }
}
