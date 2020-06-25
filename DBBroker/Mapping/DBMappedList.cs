using System;

namespace DBBroker.Mapping
{
    /// <summary>
    /// <para>Maps the references of a one-to-many relationship represented by a property of type System.Collections.Generic.List</para>
    /// <remarks>DBBroker will not take care of the state of elements in List individually, only the references represented by it.</remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DBMappedList : Map
    {
        /// <summary>
        /// Table in which this one-to-many relationship should be stored
        /// </summary>
        public string RelationshipTable { get; set; }

        /// <summary>
        /// Name of column that holds the parent Id values
        /// </summary>
        public string ParentColumnIds { get; set; }

        /// <summary>
        /// Name of column that holds the children Id values
        /// </summary>
        public string ChildrenColumnIds { get; set; }
    }
}
