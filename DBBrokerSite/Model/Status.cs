using DBBroker.Mapping;

namespace DBBrokerSite.Model
{
    /// <summary>
    /// Created, Open, Resolved, Inactive
    /// </summary>
    [DBMappedClass(Table ="Status", PrimaryKey ="IdStatus")]
    public class Status : BusinessObject
    {
        /// <summary>
        /// Created, Open, Resolved, Inactive
        /// </summary>
        public string Description { get; set; }
    }
}