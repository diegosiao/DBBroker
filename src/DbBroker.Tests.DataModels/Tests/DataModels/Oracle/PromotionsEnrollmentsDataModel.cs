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
    [Table("PROMOTIONS_ENROLLMENTS", Schema = "DBBROKER")]
    public class PromotionsEnrollmentsDataModel : DataModel<PromotionsEnrollmentsDataModel>
    {
    
        private decimal _Id;

        [Key, Column("ID"), ColumnType(OracleDbType.Decimal)]
        public decimal Id
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

        private decimal _PromotionId;

        [Column("PROMOTION_ID"), ColumnType(OracleDbType.Decimal)]
        public decimal PromotionId
        { 
            get
            {
                return _PromotionId;
            } 
            set
            {
                _IsNotPristine[nameof(PromotionId)] = true;
                _PromotionId = value;
            }
        }

    
    [DataModelReference(nameof(CustomerId), schemaName: "DBBROKER", tableName: "PROMOTIONS_ENROLLMENTS", columnName: "CUSTOMER_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS")]
    public CustomersDataModel? CustomerIdRef { get; set; }


    [DataModelReference(nameof(PromotionId), schemaName: "DBBROKER", tableName: "PROMOTIONS_ENROLLMENTS", columnName: "PROMOTION_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "PROMOTIONS")]
    public PromotionsDataModel? PromotionIdRef { get; set; }


    
        static PromotionsEnrollmentsDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
