using DBBroker.Mapping;

namespace DBBrokerClient.Model
{
    [DBMappedClass(Table="Cars", PrimaryKey="IdCar")]
    public class Car : BusinessObject
    {
        public string Model { get; set; }
    }
}
