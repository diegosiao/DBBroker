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
    [Table("OrdersProducts", Schema = "dbo")]
    public class OrdersproductsDataModel : DataModel<OrdersproductsDataModel>
    {
    
        private Guid _Id;

        [Key, Column("Id"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Id
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

        private Guid _Orderid;

        [Column("OrderId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Orderid
        { 
            get
            {
                return _Orderid;
            } 
            set
            {
                _IsNotPristine[nameof(Orderid)] = true;
                _Orderid = value;
            }
        }

        private Guid _Productid;

        [Column("ProductId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Productid
        { 
            get
            {
                return _Productid;
            } 
            set
            {
                _IsNotPristine[nameof(Productid)] = true;
                _Productid = value;
            }
        }

    
    [DataModelReference(nameof(Orderid), schemaName: "dbo", tableName: "OrdersProducts", columnName: "OrderId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Orders")]
    public OrdersDataModel? OrderidRef { get; set; }


    [DataModelReference(nameof(Productid), schemaName: "dbo", tableName: "OrdersProducts", columnName: "ProductId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Products")]
    public ProductsDataModel? ProductidRef { get; set; }


    
        static OrdersproductsDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
