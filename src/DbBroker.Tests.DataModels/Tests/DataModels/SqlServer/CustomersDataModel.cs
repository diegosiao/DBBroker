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
    [Table("Customers", Schema = "dbo")]
    public class CustomersDataModel : DataModel<CustomersDataModel>
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

        private Guid? _Addressid;

        [Column("AddressId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid? Addressid
        { 
            get
            {
                return _Addressid;
            } 
            set
            {
                _IsNotPristine[nameof(Addressid)] = true;
                _Addressid = value;
            }
        }

        private string? _Name;

        [Column("Name"), ColumnType(SqlDbType.VarChar)]
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

        [Column("Email"), ColumnType(SqlDbType.VarChar)]
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

        [Column("Birthday"), ColumnType(SqlDbType.Date)]
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

        private int? _Orderstotal;

        [Column("OrdersTotal"), ColumnType(SqlDbType.Int)]
        public int? Orderstotal
        { 
            get
            {
                return _Orderstotal;
            } 
            set
            {
                _IsNotPristine[nameof(Orderstotal)] = true;
                _Orderstotal = value;
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

    
    [DataModelReference(nameof(Addressid), schemaName: "dbo", tableName: "Customers", columnName: "AddressId", columnAllowNulls: true, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Addresses")]
    public AddressesDataModel? AddressidRef { get; set; }


    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "Customers", columnName: "Id", refColumnName: "CustomerId", refSchemaName: "dbo", refTableName: "CustomersNotes", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(CustomersnotesDataModel))]
    public IEnumerable<CustomersnotesDataModel>? CustomersnotesCustomeridRefs { get; set; }


    [DataModelCollectionReference(schemaName: "dbo", tableName: "Customers", columnName: "Id", refColumnName: "CustomerId", refSchemaName: "dbo", refTableName: "Orders", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersDataModel))]
    public IEnumerable<OrdersDataModel>? OrdersCustomeridRefs { get; set; }


    [DataModelCollectionReference(schemaName: "dbo", tableName: "Customers", columnName: "Id", refColumnName: "CustomerId", refSchemaName: "dbo", refTableName: "PromotionsEnrollments", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(PromotionsenrollmentsDataModel))]
    public IEnumerable<PromotionsenrollmentsDataModel>? PromotionsenrollmentsCustomeridRefs { get; set; }


        static CustomersDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
