using System;

namespace DBBroker.Mapping
{
    /// <summary>
    /// Attribute to map the domain classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DBMappedClass : Map
    {
        /// <summary>
        /// Table to which this class should be mapped in the database
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// <para>For Oracle context, the sequence associated with the primary key</para>
        /// <para>If not informed, you will not be aware of the created Id after INSERT commands automatically</para>
        /// </summary>
        public string Sequence { get; set; }

        /// <summary>
        /// Primary key of this class in the table specified
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// Determines the cache strategy for this class
        /// </summary>
        internal CacheStrategy Cache { get; set; }
    }
}
