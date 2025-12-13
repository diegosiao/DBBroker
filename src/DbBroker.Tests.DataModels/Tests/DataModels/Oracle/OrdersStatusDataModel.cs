using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oracle.ManagedDataAccess.Client;
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;
using System.Collections.Generic;

namespace DbBroker.Tests.DataModels.Oracle
{
    [Table("ORDERS_STATUS", Schema = "DBBROKER")]
    public class OrdersStatusDataModel : DataModel<OrdersStatusDataModel>
    {
    
        private decimal _Id;

        [Key, Column("ID"), ColumnType(OracleDbType.Decimal)]
        public decimal Id
        { 
            get
            {
                return _Id;
            } 
            set
            {
                _IsNotPristine[nameof(Id)] = true;
                _Id = value;
            }
        }

        private string? _Status;

        [Column("STATUS"), ColumnType(OracleDbType.Varchar2)]
        public string? Status
        { 
            get
            {
                return _Status;
            } 
            set
            {
                _IsNotPristine[nameof(Status)] = true;
                _Status = value;
            }
        }

    
    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ORDERS_STATUS", columnName: "ID", refColumnName: "STATUS_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersStatusIdRefs { get; set; }


        static OrdersStatusDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
