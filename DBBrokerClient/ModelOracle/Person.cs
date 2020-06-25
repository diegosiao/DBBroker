using DBBroker.Mapping;
using System;

namespace DBBrokerClient.ModelOracle
{
    [DBMappedClass(Table="PERSONS", PrimaryKey="IDPERSON", Sequence = "PERSONS_SEQ")]
    public class Person : BusinessObject
    {   
        public string Name { get; set; }

        //public Person ClosestFriend { get; set; }

        public DateTime? Birthday { get; set; }

        public decimal Salary { get; set; }

        //public double SomeDouble { get; set; }

        //public long SomeLong { get; set; }

        public bool IsFriend { get; set; }

        //public Car Car { get; set; }
        //public Car PriorCar { get; set; }

        //public string PhotoFileName { get; set; }
        //public byte[] PhotoFileBytes { get; set; }

        //[DBReadOnly(DBDefaultValue="GETDATE()")]
        //public DateTime Registration { get; set; }

        //[DBMappedList(RelationshipTable = "Persons_Cars", ParentColumnIds = "IdPerson", ChildrenColumnIds = "IdCar")]
        //public List<Car> Cars { get; set; }
    }
}
