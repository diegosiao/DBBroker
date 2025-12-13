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
    [Table("PromotionsEnrollments", Schema = "dbo")]
    public class PromotionsenrollmentsDataModel : DataModel<PromotionsenrollmentsDataModel>
    {
    
        private int _Id;

        [Key, Column("Id"), ColumnType(SqlDbType.Int)]
        public int Id
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

        private int _Promotionid;

        [Column("PromotionId"), ColumnType(SqlDbType.Int)]
        public int Promotionid
        { 
            get
            {
                return _Promotionid;
            } 
            set
            {
                _IsNotPristine[nameof(Promotionid)] = true;
                _Promotionid = value;
            }
        }

    
    [DataModelReference(nameof(Customerid), schemaName: "dbo", tableName: "PromotionsEnrollments", columnName: "CustomerId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Customers")]
    public CustomersDataModel? CustomeridRef { get; set; }


    [DataModelReference(nameof(Promotionid), schemaName: "dbo", tableName: "PromotionsEnrollments", columnName: "PromotionId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Promotions")]
    public PromotionsDataModel? PromotionidRef { get; set; }


    
        static PromotionsenrollmentsDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
