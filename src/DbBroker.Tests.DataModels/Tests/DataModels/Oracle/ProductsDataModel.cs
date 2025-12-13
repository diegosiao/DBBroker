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
    [Table("PRODUCTS", Schema = "DBBROKER")]
    public class ProductsDataModel : DataModel<ProductsDataModel>
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

        private string? _ProductName;

        [Column("PRODUCT_NAME"), ColumnType(OracleDbType.Varchar2)]
        public string? ProductName
        { 
            get
            {
                return _ProductName;
            } 
            set
            {
                _IsNotPristine[nameof(ProductName)] = true;
                _ProductName = value;
            }
        }

        private decimal? _Price;

        [Column("PRICE"), ColumnType(OracleDbType.Decimal)]
        public decimal? Price
        { 
            get
            {
                return _Price;
            } 
            set
            {
                _IsNotPristine[nameof(Price)] = true;
                _Price = value;
            }
        }

    
    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "PRODUCTS", columnName: "ID", refColumnName: "PRODUCT_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS_PRODUCTS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersProductsDataModel))]
    public IEnumerable<OrdersProductsDataModel>? OrdersProductsProductIdRefs { get; set; }


        static ProductsDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
