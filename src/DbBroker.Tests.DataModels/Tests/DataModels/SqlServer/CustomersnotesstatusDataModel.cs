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
    [Table("CustomersNotesStatus", Schema = "dbo")]
    public class CustomersnotesstatusDataModel : DataModel<CustomersnotesstatusDataModel>
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

        private string? _Status;

        [Column("Status"), ColumnType(SqlDbType.VarChar)]
        public string? Status
        { 
            get
            {
                return _Status;
            } 
            set
            {
                _IsNotPristine[nameof(Status)] = true;
                _Status = value;
            }
        }

    
    
    [DataModelCollectionReference(schemaName: "dbo", tableName: "CustomersNotesStatus", columnName: "Id", refColumnName: "StatusId", refSchemaName: "dbo", refTableName: "CustomersNotes", refTablePrimaryKeyColumnName: "Id", dataModelType: typeof(CustomersnotesDataModel))]
    public IEnumerable<CustomersnotesDataModel>? CustomersnotesStatusidRefs { get; set; }


        static CustomersnotesstatusDataModel()
        {
            Provider = SupportedDatabaseProviders.SqlServer;
            SqlInsertTemplateTypeFullName = "DbBroker.Model.Providers.SqlServerExplicitKeySqlInsertTemplate";
            SqlInsertTemplateTypeArguments = new object[] {};
        }
    }
}
