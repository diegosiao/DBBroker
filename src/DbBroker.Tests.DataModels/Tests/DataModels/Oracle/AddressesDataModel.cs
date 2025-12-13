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
    [Table("ADDRESSES", Schema = "DBBROKER")]
    public class AddressesDataModel : DataModel<AddressesDataModel>
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

        private string? _Street;

        [Column("STREET"), ColumnType(OracleDbType.Varchar2)]
        public string? Street
        { 
            get
            {
                return _Street;
            } 
            set
            {
                _IsNotPristine[nameof(Street)] = true;
                _Street = value;
            }
        }

        private string? _City;

        [Column("CITY"), ColumnType(OracleDbType.Varchar2)]
        public string? City
        { 
            get
            {
                return _City;
            } 
            set
            {
                _IsNotPristine[nameof(City)] = true;
                _City = value;
            }
        }

        private string? _State;

        [Column("STATE"), ColumnType(OracleDbType.Varchar2)]
        public string? State
        { 
            get
            {
                return _State;
            } 
            set
            {
                _IsNotPristine[nameof(State)] = true;
                _State = value;
            }
        }

        private string? _PostalCode;

        [Column("POSTAL_CODE"), ColumnType(OracleDbType.Varchar2)]
        public string? PostalCode
        { 
            get
            {
                return _PostalCode;
            } 
            set
            {
                _IsNotPristine[nameof(PostalCode)] = true;
                _PostalCode = value;
            }
        }

        private string? _Country;

        [Column("COUNTRY"), ColumnType(OracleDbType.Varchar2)]
        public string? Country
        { 
            get
            {
                return _Country;
            } 
            set
            {
                _IsNotPristine[nameof(Country)] = true;
                _Country = value;
            }
        }

    
    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ADDRESSES", columnName: "ID", refColumnName: "ADDRESS_ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(CustomersDataModel))]
    public IEnumerable<CustomersDataModel>? CustomersAddressIdRefs { get; set; }


    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ADDRESSES", columnName: "ID", refColumnName: "BILLING_ADDRESS_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersBillingAddressIdRefs { get; set; }


    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "ADDRESSES", columnName: "ID", refColumnName: "SHIPPING_ADDRESS_ID", refSchemaName: "DBBROKER", refTableName: "ORDERS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersShippingAddressIdRefs { get; set; }


        static AddressesDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
