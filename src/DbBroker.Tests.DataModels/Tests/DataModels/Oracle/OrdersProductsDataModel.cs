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
    [Table("ORDERS_PRODUCTS", Schema = "DBBROKER")]
    public class OrdersProductsDataModel : DataModel<OrdersProductsDataModel>
    {
    
        private byte[]? _Id;

        [Key, Column("ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? Id
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

        private byte[]? _OrderId;

        [Column("ORDER_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? OrderId
        { 
            get
            {
                return _OrderId;
            } 
            set
            {
                _IsNotPristine[nameof(OrderId)] = true;
                _OrderId = value;
            }
        }

        private byte[]? _ProductId;

        [Column("PRODUCT_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? ProductId
        { 
            get
            {
                return _ProductId;
            } 
            set
            {
                _IsNotPristine[nameof(ProductId)] = true;
                _ProductId = value;
            }
        }

    
    [DataModelReference(nameof(OrderId), schemaName: "DBBROKER", tableName: "ORDERS_PRODUCTS", columnName: "ORDER_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ORDERS")]
    public OrdersDataModel? OrderIdRef { get; set; }


    [DataModelReference(nameof(ProductId), schemaName: "DBBROKER", tableName: "ORDERS_PRODUCTS", columnName: "PRODUCT_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "PRODUCTS")]
    public ProductsDataModel? ProductIdRef { get; set; }


    
        static OrdersProductsDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
