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
    [Table("Addresses", Schema = "dbo")]
    public class AddressesDataModel : DataModel<AddressesDataModel>
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

        private string? _Street;

        [Column("Street"), ColumnType(SqlDbType.VarChar)]
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

        [Column("City"), ColumnType(SqlDbType.VarChar)]
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

        [Column("State"), ColumnType(SqlDbType.VarChar)]
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

        private string? _Postalcode;

        [Column("PostalCode"), ColumnType(SqlDbType.VarChar)]
        public string? Postalcode
        { 
            get
            {
                return _Postalcode;
            } 
            set
            {
                _IsNotPristine[nameof(Postalcode)] = true;
                _Postalcode = value;
            }
        }

        private string? _Country;

        [Column("Country"), ColumnType(SqlDbType.VarChar)]
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

    
    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "Addresses", columnName: "Id", refColumnName: "AddressId", refSchemaName: "dbo", refTableName: "Customers", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(CustomersDataModel))]
    public IEnumerable<CustomersDataModel>? CustomersAddressidRefs { get; set; }


    [DataModelCollectionReference(schemaName: "dbo", tableName: "Addresses", columnName: "Id", refColumnName: "BillingAddressId", refSchemaName: "dbo", refTableName: "Orders", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersBillingaddressidRefs { get; set; }


    [DataModelCollectionReference(schemaName: "dbo", tableName: "Addresses", columnName: "Id", refColumnName: "ShippingAddressId", refSchemaName: "dbo", refTableName: "Orders", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersShippingaddressidRefs { get; set; }


        static AddressesDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
