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
    [Table("ORDERS", Schema = "DBBROKER")]
    public class OrdersDataModel : DataModel<OrdersDataModel>
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

        private byte[]? _CustomerId;

        [Column("CUSTOMER_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? CustomerId
        { 
            get
            {
                return _CustomerId;
            } 
            set
            {
                _IsNotPristine[nameof(CustomerId)] = true;
                _CustomerId = value;
            }
        }

        private byte[]? _BillingAddressId;

        [Column("BILLING_ADDRESS_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? BillingAddressId
        { 
            get
            {
                return _BillingAddressId;
            } 
            set
            {
                _IsNotPristine[nameof(BillingAddressId)] = true;
                _BillingAddressId = value;
            }
        }

        private byte[]? _ShippingAddressId;

        [Column("SHIPPING_ADDRESS_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? ShippingAddressId
        { 
            get
            {
                return _ShippingAddressId;
            } 
            set
            {
                _IsNotPristine[nameof(ShippingAddressId)] = true;
                _ShippingAddressId = value;
            }
        }

        private decimal? _StatusId;

        [Column("STATUS_ID"), ColumnType(OracleDbType.Decimal)]
        public decimal? StatusId
        { 
            get
            {
                return _StatusId;
            } 
            set
            {
                _IsNotPristine[nameof(StatusId)] = true;
                _StatusId = value;
            }
        }

        private DateTime _CreatedAt;

        [Column("CREATED_AT"), ColumnType(OracleDbType.Date)]
        public DateTime CreatedAt
        { 
            get
            {
                return _CreatedAt;
            } 
            set
            {
                _IsNotPristine[nameof(CreatedAt)] = true;
                _CreatedAt = value;
            }
        }

        private string? _CreatedBy;

        [Column("CREATED_BY"), ColumnType(OracleDbType.Varchar2)]
        public string? CreatedBy
        { 
            get
            {
                return _CreatedBy;
            } 
            set
            {
                _IsNotPristine[nameof(CreatedBy)] = true;
                _CreatedBy = value;
            }
        }

        private DateTime? _ModifiedAt;

        [Column("MODIFIED_AT"), ColumnType(OracleDbType.Date)]
        public DateTime? ModifiedAt
        { 
            get
            {
                return _ModifiedAt;
            } 
            set
            {
                _IsNotPristine[nameof(ModifiedAt)] = true;
                _ModifiedAt = value;
            }
        }

        private string? _ModifiedBy;

        [Column("MODIFIED_BY"), ColumnType(OracleDbType.Varchar2)]
        public string? ModifiedBy
        { 
            get
            {
                return _ModifiedBy;
            } 
            set
            {
                _IsNotPristine[nameof(ModifiedBy)] = true;
                _ModifiedBy = value;
            }
        }

    
    [DataModelReference(nameof(CustomerId), schemaName: "DBBROKER", tableName: "ORDERS", columnName: "CUSTOMER_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS")]
    public CustomersDataModel? CustomerIdRef { get; set; }


    [DataModelReference(nameof(BillingAddressId), schemaName: "DBBROKER", tableName: "ORDERS", columnName: "BILLING_ADDRESS_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ADDRESSES")]
    public AddressesDataModel? BillingAddressIdRef { get; set; }


    [DataModelReference(nameof(ShippingAddressId), schemaName: "DBBROKER", tableName: "ORDERS", columnName: "SHIPPING_ADDRESS_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ADDRESSES")]
    public AddressesDataModel? ShippingAddressIdRef { get; set; }


    [DataModelReference(nameof(StatusId), schemaName: "DBBROKER", tableName: "ORDERS", columnName: "STATUS_ID", columnAllowNulls: true, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ORDERS_STATUS")]
    public OrdersStatusDataModel? StatusIdRef { get; set; }


    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ORDERS", columnName: "ID", refColumnName: "ORDER_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS_NOTES", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersNotesDataModel))]
    public IEnumerable<OrdersNotesDataModel>? OrdersNotesOrderIdRefs { get; set; }


    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ORDERS", columnName: "ID", refColumnName: "ORDER_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS_PRODUCTS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersProductsDataModel))]
    public IEnumerable<OrdersProductsDataModel>? OrdersProductsOrderIdRefs { get; set; }


        static OrdersDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
