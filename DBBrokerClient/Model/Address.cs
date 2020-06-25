using DBBroker.Mapping;

namespace DBBrokerClient.Model
{
    [DBMappedClass(Table="PersonAddresses", PrimaryKey="IdAddress")]
    public class Address : BusinessObject
    {

        public Person Person { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
    }
}
