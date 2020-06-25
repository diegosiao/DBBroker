using DBBroker.Mapping;
using DBBroker.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DBBroker.Engine
{
    internal class MapeamentoCarga
    {
        public enum TipoPropriedade { Dominio, List, Primitivo }

        public PropertyInfo Prop { get; set; }

        public string PropName { get; set; }

        public int Ordinal { get; set; }

        public TipoPropriedade TipoCarga { get; set; }

        public DBMappedClass Mapeamento { get; set; }

        public DBMappedList MapeamentoLista { get; set; }

        public string MapeadoPara { get; set; }

        public Type TipoDeDominio { get; private set; }

        public Dictionary<long, object> IdsChaveEstrangeira { get; set; }

        public string SqlStmt
        {
            get
            {
                if (TipoCarga == TipoPropriedade.Dominio)
                {
                    if (IdsChaveEstrangeira == null || IdsChaveEstrangeira.Count == 0)
                        return " {Erro4785} ";

                    return "SELECT * FROM " + Mapeamento.Table + " WHERE " + Mapeamento.PrimaryKey + InStmtBuilder(IdsChaveEstrangeira.Keys);
                }
                else if (TipoCarga == TipoPropriedade.List)
                {
                    if (Mapeamento == null || string.IsNullOrEmpty(Mapeamento.Table) || string.IsNullOrEmpty(Mapeamento.PrimaryKey))
                        throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, TipoDeDominio));

                    return " SELECT * " +
                           " FROM " + Mapeamento.Table +
                           " WHERE " + Mapeamento.Table + "." + Mapeamento.PrimaryKey + " IN (" +
                                                                                              " SELECT " + MapeamentoLista.ChildrenColumnIds +
                                                                                              " FROM " + MapeamentoLista.RelationshipTable +
                                                                                              " WHERE " + MapeamentoLista.ParentColumnIds + InStmtBuilder(IdsChaveEstrangeira.Keys) + ")";
                }
                else return null;
            }
        }

        public MapeamentoCarga() { }

        public MapeamentoCarga(string prop, int ordinal)
        {
            IdsChaveEstrangeira = new Dictionary<long, object>();
            TipoCarga = TipoPropriedade.Primitivo;
            PropName = prop;
            Ordinal = ordinal;
        }

        public MapeamentoCarga(PropertyInfo prop, int ordinal, DBMappedClass mapeamento, string mapeadopara)
        {
            IdsChaveEstrangeira = new Dictionary<long, object>();

            Prop = prop;

            PropName = prop.Name;
            Ordinal = ordinal;
            TipoCarga = TipoPropriedade.Dominio;
            Mapeamento = mapeamento;
            MapeadoPara = mapeadopara;
        }

        public MapeamentoCarga(PropertyInfo prop, DBMappedList mapeamento)
        {
            IdsChaveEstrangeira = new Dictionary<long, object>();

            Prop = prop;
            TipoDeDominio = prop.PropertyType.GetGenericArguments()[0];

            object[] o = TipoDeDominio.GetCustomAttributes(typeof(DBMappedClass), true);

            if (o.Length == 0)
                throw new DBBrokerException(string.Format(Resources.ErrorIncompleteMap, TipoDeDominio.FullName));

            Mapeamento = (DBMappedClass)o[0];

            PropName = prop.Name;
            Ordinal = -1;
            TipoCarga = TipoPropriedade.List;
            MapeamentoLista = mapeamento;
        }

        public string InStmtBuilder(ICollection values)
        {
            string stmt = " IN (";
            foreach (object id in values)
            {
                if (id == null || id.ToString().Equals("0"))
                    continue;
                stmt += id.ToString() + ", ";
            }
            return stmt.Substring(0, stmt.Length - 2) + ") ";
        }
    }
}
