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
    [Table("CustomersNotes", Schema = "dbo")]
    public class CustomersnotesDataModel : DataModel<CustomersnotesDataModel>
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

        private Guid? _Customerid;

        [Column("CustomerId"), ColumnType(SqlDbType.UniqueIdentifier)]
        public Guid? Customerid
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

        private int? _Statusid;

        [Column("StatusId"), ColumnType(SqlDbType.Int)]
        public int? Statusid
        { 
            get
            {
                return _Statusid;
            } 
            set
            {
                _IsNotPristine[nameof(Statusid)] = true;
                _Statusid = value;
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

    
    [DataModelReference(nameof(Customerid), schemaName: "dbo", tableName: "CustomersNotes", columnName: "CustomerId", columnAllowNulls: true, refColumnName: "Id", refSchemaName: "dbo", refTableName: "Customers")]
    public CustomersDataModel? CustomeridRef { get; set; }


    [DataModelReference(nameof(Statusid), schemaName: "dbo", tableName: "CustomersNotes", columnName: "StatusId", columnAllowNulls: true, refColumnName: "Id", refSchemaName: "dbo", refTableName: "CustomersNotesStatus")]
    public CustomersnotesstatusDataModel? StatusidRef { get; set; }


    
        static CustomersnotesDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
