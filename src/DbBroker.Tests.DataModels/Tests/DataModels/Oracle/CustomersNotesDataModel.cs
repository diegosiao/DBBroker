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
    [Table("CUSTOMERS_NOTES", Schema = "DBBROKER")]
    public class CustomersNotesDataModel : DataModel<CustomersNotesDataModel>
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

        private decimal? _StatusId;

        [Column("STATUS_ID"), ColumnType(OracleDbType.Decimal)]
        public decimal? StatusId
        { 
            get
            {
                return _StatusId;
            } 
            set
            {
                _IsNotPristine[nameof(StatusId)] = true;
                _StatusId = value;
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

    
    [DataModelReference(nameof(CustomerId), schemaName: "DBBROKER", tableName: "CUSTOMERS_NOTES", columnName: "CUSTOMER_ID", columnAllowNulls: true, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS")]
    public CustomersDataModel? CustomerIdRef { get; set; }


    [DataModelReference(nameof(StatusId), schemaName: "DBBROKER", tableName: "CUSTOMERS_NOTES", columnName: "STATUS_ID", columnAllowNulls: true, refColumnName: "ID", refSchemaName: "DBBROKER", refTableName: "CUSTOMERS_NOTES_STATUS")]
    public CustomersNotesStatusDataModel? StatusIdRef { get; set; }


    
        static CustomersNotesDataModel()
        {
            Provider = SupportedDatabaseProviders.Oracle;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.Oracle.OracleExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
