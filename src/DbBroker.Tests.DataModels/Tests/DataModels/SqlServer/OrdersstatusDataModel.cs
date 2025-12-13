using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;
using System.Collections.Generic;

namespace DbBroker.Tests.DataModels.SqlServer
{
    [Table("OrdersStatus", Schema = "dbo")]
    public class OrdersstatusDataModel : DataModel<OrdersstatusDataModel>
    {
    
        private int _Id;

        [Key, Column("Id"), ColumnType(SqlDbType.Int)]
        public int Id
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

        [Column("Status"), ColumnType(SqlDbType.VarChar)]
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

    
    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "OrdersStatus", columnName: "Id", refColumnName: "StatusId", refSchemaName: "dbo", refTableName: "Orders", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersStatusidRefs { get; set; }


        static OrdersstatusDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
