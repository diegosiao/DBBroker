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
    [Table("Promotions", Schema = "dbo")]
    public class PromotionsDataModel : DataModel<PromotionsDataModel>
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

        private string? _Title;

        [Column("Title"), ColumnType(SqlDbType.VarChar)]
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

        [Column("Expiration"), ColumnType(SqlDbType.DateTime)]
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

    
    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "Promotions", columnName: "Id", refColumnName: "PromotionId", refSchemaName: "dbo", refTableName: "PromotionsEnrollments", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(PromotionsenrollmentsDataModel))]
    public IEnumerable<PromotionsenrollmentsDataModel>? PromotionsenrollmentsPromotionidRefs { get; set; }


        static PromotionsDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
