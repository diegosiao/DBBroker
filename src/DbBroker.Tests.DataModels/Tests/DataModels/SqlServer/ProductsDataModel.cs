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
    [Table("Products", Schema = "dbo")]
    public class ProductsDataModel : DataModel<ProductsDataModel>
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

        private string? _Productname;

        [Column("ProductName"), ColumnType(SqlDbType.VarChar)]
        public string? Productname
        { 
            get
            {
                return _Productname;
            } 
            set
            {
                _IsNotPristine[nameof(Productname)] = true;
                _Productname = value;
            }
        }

        private decimal _Price;

        [Column("Price"), ColumnType(SqlDbType.Money)]
        public decimal Price
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

    
    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "Products", columnName: "Id", refColumnName: "ProductId", refSchemaName: "dbo", refTableName: "OrdersProducts", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(OrdersproductsDataModel))]
    public IEnumerable<OrdersproductsDataModel>? OrdersproductsProductidRefs { get; set; }


        static ProductsDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
