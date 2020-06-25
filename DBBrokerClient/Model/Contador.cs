using DBBroker.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBBrokerClient.Model
{
    [DBMappedClass(Table="NENHUMA", PrimaryKey="SQ_PESSOA")]
    public class Contador
    {
        public int Id { get; set; }

        public string NM_FORMAL { get; set; }

        public string NU_CNPF { get; set; }
    }
}
