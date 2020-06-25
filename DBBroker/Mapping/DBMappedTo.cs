using System;

namespace DBBroker.Mapping
{
    /// <summary>
    /// Maps a property to a not conventional column name in database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBMappedTo : Map
    {
        /// <summary>
        /// Name of the database column name mapped to that property
        /// </summary>
        public string Column { get; set; }
    }
}
