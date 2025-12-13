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
    [Table("CUSTOMERS", Schema = "DBBROKER")]
    public class CustomersDataModel : DataModel<CustomersDataModel>
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

        private byte[]? _AddressId;

        [Column("ADDRESS_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? AddressId
        { 
            get
            {
                return _AddressId;
            } 
            set
            {
                _IsNotPristine[nameof(AddressId)] = true;
                _AddressId = value;
            }
        }

        private string? _Name;

        [Column("NAME"), ColumnType(OracleDbType.Varchar2)]
        public string? Name
        { 
            get
            {
                return _Name;
            } 
            set
            {
                _IsNotPristine[nameof(Name)] = true;
                _Name = value;
            }
        }

        private string? _Email;

        [Column("EMAIL"), ColumnType(OracleDbType.Varchar2)]
        public string? Email
        { 
            get
            {
                return _Email;
            } 
            set
            {
                _IsNotPristine[nameof(Email)] = true;
                _Email = value;
            }
        }

        private DateTime? _Birthday;

        [Column("BIRTHDAY"), ColumnType(OracleDbType.Date)]
        public DateTime? Birthday
        { 
            get
            {
                return _Birthday;
            } 
            set
            {
                _IsNotPristine[nameof(Birthday)] = true;
                _Birthday = value;
            }
        }

        private decimal? _OrdersTotal;

        [Column("ORDERS_TOTAL"), ColumnType(OracleDbType.Decimal)]
        public decimal? OrdersTotal
        { 
            get
            {
                return _OrdersTotal;
            } 
            set
            {
                _IsNotPristine[nameof(OrdersTotal)] = true;
                _OrdersTotal = value;
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

    
    [DataModelReference(nameof(AddressId), schemaName: "DBBROKER", tableName: "CUSTOMERS", columnName: "ADDRESS_ID", columnAllowNulls: true, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ADDRESSES")]
    public AddressesDataModel? AddressIdRef { get; set; }


    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "CUSTOMERS", columnName: "ID", refColumnName: "CUSTOMER_ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS_NOTES", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(CustomersNotesDataModel))]
    public IEnumerable<CustomersNotesDataModel>? CustomersNotesCustomerIdRefs { get; set; }


    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "CUSTOMERS", columnName: "ID", refColumnName: "CUSTOMER_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersCustomerIdRefs { get; set; }


    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "CUSTOMERS", columnName: "ID", refColumnName: "CUSTOMER_ID", refSchemaName: "DBBROKER", refTableName: "PROMOTIONS_ENROLLMENTS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(PromotionsEnrollmentsDataModel))]
    public IEnumerable<PromotionsEnrollmentsDataModel>? PromotionsEnrollmentsCustomerIdRefs { get; set; }


        static CustomersDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
