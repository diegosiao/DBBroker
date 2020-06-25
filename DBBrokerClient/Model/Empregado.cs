using DBBroker.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBBrokerClient.Model
{
    [DBMappedClass(Table="EMPREGADOO", PrimaryKey="ID_EMPREGADO")]
    public class Empregado
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        [DBMappedTo(Column="ID_CARGO")]
        public Cargo Cargo { get; set; }

        [DBMappedList(
            RelationshipTable="EMPREGADOO",
            ParentColumnIds="ID_EMPREGADO",
            ChildrenColumnIds="ID_CARGO")]
        public List<Cargo> Cargos { get; set; }
    }

    [DBMappedClass(Table="CARGO", PrimaryKey="ID_CARGO")]
    public class Cargo
    {
        public int Id { get; set; }

        public string Nome_Cargo { get; set; }
    }
}
