using DBBroker.Mapping;

namespace DBBrokerClient.ModelMySQL
{
    [DBMappedClass(Table ="cars",PrimaryKey ="IdCar")]
    public class MyCar
    {
        public int Id { get; set; }
        public string Model { get; set; }
    }
}