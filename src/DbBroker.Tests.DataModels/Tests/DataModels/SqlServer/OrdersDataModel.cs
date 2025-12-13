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
    [Table("Orders", Schema = "dbo")]
    public class OrdersDataModel : DataModel<OrdersDataModel>
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

        private Guid _Customerid;

        [Column("CustomerId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Customerid
        { 
            get
            {
                return _Customerid;
            } 
            set
            {
                _IsNotPristine[nameof(Customerid)] = true;
                _Customerid = value;
            }
        }

        private Guid _Billingaddressid;

        [Column("BillingAddressId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Billingaddressid
        { 
            get
            {
                return _Billingaddressid;
            } 
            set
            {
                _IsNotPristine[nameof(Billingaddressid)] = true;
                _Billingaddressid = value;
            }
        }

        private Guid _Shippingaddressid;

        [Column("ShippingAddressId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Shippingaddressid
        { 
            get
            {
                return _Shippingaddressid;
            } 
            set
            {
                _IsNotPristine[nameof(Shippingaddressid)] = true;
                _Shippingaddressid = value;
            }
        }

        private int? _Statusid;

        [Column("StatusId"), ColumnType(SqlDbType.Int)]
        public int? Statusid
        { 
            get
            {
                return _Statusid;
            } 
            set
            {
                _IsNotPristine[nameof(Statusid)] = true;
                _Statusid = value;
            }
        }

        private DateTime _Createdat;

        [Column("CreatedAt"), ColumnType(SqlDbType.DateTime)]
        public DateTime Createdat
        { 
            get
            {
                return _Createdat;
            } 
            set
            {
                _IsNotPristine[nameof(Createdat)] = true;
                _Createdat = value;
            }
        }

        private string? _Createdby;

        [Column("CreatedBy"), ColumnType(SqlDbType.VarChar)]
        public string? Createdby
        { 
            get
            {
                return _Createdby;
            } 
            set
            {
                _IsNotPristine[nameof(Createdby)] = true;
                _Createdby = value;
            }
        }

        private DateTime? _Modifiedat;

        [Column("ModifiedAt"), ColumnType(SqlDbType.DateTime)]
        public DateTime? Modifiedat
        { 
            get
            {
                return _Modifiedat;
            } 
            set
            {
                _IsNotPristine[nameof(Modifiedat)] = true;
                _Modifiedat = value;
            }
        }

        private string? _Modifiedby;

        [Column("ModifiedBy"), ColumnType(SqlDbType.VarChar)]
        public string? Modifiedby
        { 
            get
            {
                return _Modifiedby;
            } 
            set
            {
                _IsNotPristine[nameof(Modifiedby)] = true;
                _Modifiedby = value;
            }
        }

    
    [DataModelReference(nameof(Customerid), schemaName: "dbo", tableName: "Orders", columnName: "CustomerId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Customers")]
    public CustomersDataModel? CustomeridRef { get; set; }


    [DataModelReference(nameof(Billingaddressid), schemaName: "dbo", tableName: "Orders", columnName: "BillingAddressId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Addresses")]
    public AddressesDataModel? BillingaddressidRef { get; set; }


    [DataModelReference(nameof(Shippingaddressid), schemaName: "dbo", tableName: "Orders", columnName: "ShippingAddressId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Addresses")]
    public AddressesDataModel? ShippingaddressidRef { get; set; }


    [DataModelReference(nameof(Statusid), schemaName: "dbo", tableName: "Orders", columnName: "StatusId", columnAllowNulls: true, refColumnName: "Id", refSchemaName: "dbo", refTableName: "OrdersStatus")]
    public OrdersstatusDataModel? StatusidRef { get; set; }


    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "Orders", columnName: "Id", refColumnName: "OrderId", refSchemaName: "dbo", refTableName: "OrdersNotes", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersnotesDataModel))]
    public IEnumerable<OrdersnotesDataModel>? OrdersnotesOrderidRefs { get; set; }


    [DataModelCollectionReference(schemaName: "dbo", tableName: "Orders", columnName: "Id", refColumnName: "OrderId", refSchemaName: "dbo", refTableName: "OrdersProducts", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersproductsDataModel))]
    public IEnumerable<OrdersproductsDataModel>? OrdersproductsOrderidRefs { get; set; }


        static OrdersDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
