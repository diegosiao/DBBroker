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
    [Table("PROMOTIONS", Schema = "DBBROKER")]
    public class PromotionsDataModel : DataModel<PromotionsDataModel>
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

        private string? _Title;

        [Column("TITLE"), ColumnType(OracleDbType.Varchar2)]
        public string? Title
        { 
            get
            {
                return _Title;
            } 
            set
            {
                _IsNotPristine[nameof(Title)] = true;
                _Title = value;
            }
        }

        private DateTime _Expiration;

        [Column("EXPIRATION"), ColumnType(OracleDbType.Date)]
        public DateTime Expiration
        { 
            get
            {
                return _Expiration;
            } 
            set
            {
                _IsNotPristine[nameof(Expiration)] = true;
                _Expiration = value;
            }
        }

    
    
    [DataModelCollectionReference(schemaName: "DBBROKER", tableName: "PROMOTIONS", columnName: "ID", refColumnName: "PROMOTION_ID", refSchemaName: "DBBROKER", refTableName: "PROMOTIONS_ENROLLMENTS", refTablePrimaryKeyColumnName: "ID", dataModelType: typeof(PromotionsEnrollmentsDataModel))]
    public IEnumerable<PromotionsEnrollmentsDataModel>? PromotionsEnrollmentsPromotionIdRefs { get; set; }


        static PromotionsDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
