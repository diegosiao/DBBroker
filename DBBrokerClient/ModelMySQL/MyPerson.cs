using DBBroker.Mapping;
using System;
using System.Collections.Generic;

namespace DBBrokerClient.ModelMySQL
{
    [DBMappedClass(Table ="persons", PrimaryKey ="idperson")]
    public class MyPerson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        [DBReadOnly]
        public DateTime Cadastro { get; set; }

        [DBMappedList(RelationshipTable ="persons_cars", ParentColumnIds ="IdPerson", ChildrenColumnIds ="IdCar")]
        public List<MyCar> Cars { get; set; }
    }
}
