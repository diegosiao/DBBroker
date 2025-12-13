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
    [Table("OrdersNotes", Schema = "dbo")]
    public class OrdersnotesDataModel : DataModel<OrdersnotesDataModel>
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

        private Guid _Orderid;

        [Column("OrderId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid Orderid
        { 
            get
            {
                return _Orderid;
            } 
            set
            {
                _IsNotPristine[nameof(Orderid)] = true;
                _Orderid = value;
            }
        }

        private string? _Notecontent;

        [Column("NoteContent"), ColumnType(SqlDbType.VarChar)]
        public string? Notecontent
        { 
            get
            {
                return _Notecontent;
            } 
            set
            {
                _IsNotPristine[nameof(Notecontent)] = true;
                _Notecontent = value;
            }
        }

    
    [DataModelReference(nameof(Orderid), schemaName: "dbo", tableName: "OrdersNotes", columnName: "OrderId", columnAllowNulls: false, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Orders")]
    public OrdersDataModel? OrderidRef { get; set; }


    
        static OrdersnotesDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
