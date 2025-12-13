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
    [Table("ORDERS_NOTES", Schema = "DBBROKER")]
    public class OrdersNotesDataModel : DataModel<OrdersNotesDataModel>
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

        private byte[]? _OrderId;

        [Column("ORDER_ID"), ColumnType(OracleDbType.Raw)]
        public byte[]? OrderId
        { 
            get
            {
                return _OrderId;
            } 
            set
            {
                _IsNotPristine[nameof(OrderId)] = true;
                _OrderId = value;
            }
        }

        private string? _NoteContent;

        [Column("NOTE_CONTENT"), ColumnType(OracleDbType.Varchar2)]
        public string? NoteContent
        { 
            get
            {
                return _NoteContent;
            } 
            set
            {
                _IsNotPristine[nameof(NoteContent)] = true;
                _NoteContent = value;
            }
        }

    
    [DataModelReference(nameof(OrderId), schemaName: "DBBROKER", tableName: "ORDERS_NOTES", columnName: "ORDER_ID", columnAllowNulls: false, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "ORDERS")]
    public OrdersDataModel? OrderIdRef { get; set; }


    
        static OrdersNotesDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleSequenceSqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {"ORDERS_NOTES_SEQ"};
        }
    }
}
