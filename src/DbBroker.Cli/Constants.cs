namespace DbBroker.Cli;

public static class Constants
{
    public const string CONFIG_FILENAME = "dbbroker.config.json";

    public const string EDM_KEY_ATTRIBUTE_TEMPLATE = @"    [Key]";

    public const string EDM_PROPERTY_TEMPLATE =
@"
        private $TYPE _$NAME;

        [$KEYColumn(""$COLUMN_NAME""), ColumnType($$PROVIDER_DBTYPE$$)]
        public $TYPE $NAME
        { 
            get
            {
                return _$NAME;
            } 
            set
            {
                _IsNotPristine[nameof($NAME)] = true;
                _$NAME = value;
            }
        }";

    public const string EDM_PROPERTY_INDENTED_TEMPLATE =
@"
            private $TYPE _$NAME;

            [$KEYColumn(""$COLUMN_NAME""), ColumnType($$PROVIDER_DBTYPE$$)]
            public $TYPE $NAME
            { 
                get
                {
                    return _$NAME;
                } 
                set
                {
                    _IsNotPristine[nameof($NAME)] = true;
                    _$NAME = value;
                }
            }";

    public const string EDM_REFERENCE_TEMPLATE =
@"
    [DataModelReference(nameof($$PROPERTYNAME$$), schemaName: ""$$SCHEMANAME$$"", tableName: ""$$TABLENAME$$"", columnName: ""$$COLUMNNAME$$"", columnAllowNulls: $$COLUMNALLOWNULLS$$, refColumnName: ""$$REFCOLUMNNAME$$"", refSchemaName: ""$$REFSCHEMANAME$$"", refTableName: ""$$REFTABLENAME$$"")]
    public $$REFTYPENAME$$? $$PROPERTYNAME$$Ref { get; set; }
";

    public const string EDM_COLLECTION_REFERENCE_TEMPLATE =
@"
    [DataModelCollectionReference(schemaName: ""$$SCHEMANAME$$"", tableName: ""$$TABLENAME$$"", columnName: ""$$COLUMNNAME$$"", refColumnName: ""$$REFCOLUMNNAME$$"", refSchemaName: ""$$REFSCHEMANAME$$"", refTableName: ""$$REFTABLENAME$$"", refTablePrimaryKeyColumnName: ""$$PKCOLUMNNAME$$"", dataModelType: typeof($$REFTYPENAME$$))]
    public IEnumerable<$$REFTYPENAME$$>? $$PROPERTYNAME$$Refs { get; set; }
";

    public const string EDM_CLASS_TEMPLATE =
@"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
$$PROVIDER_CLIENT_NAMESPACE$$
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;

namespace $NAMESPACE
{
    [Table(""$TABLE"", Schema = ""$SCHEMA"")]
    public class $CLASSNAME : DataModel<$CLASSNAME>
    {
    $PROPERTIES
    $REFERENCES
    $COLLECTIONS
        static $CLASSNAME()
        {
            Provider = SupportedDatabaseProviders.$PROVIDER;
            SqlInsertTemplateTypeFullName = ""$ISQLINSERTTEMPLATETYPEFULLNAME"";
            SqlInsertTemplateTypeArguments = new object[] {$ISQLINSERTTEMPLATETYPEARGUMENTS};
        }
    }
}
";
    public const string EDM_VIEW_CLASS_TEMPLATE =
@"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
$$PROVIDER_CLIENT_NAMESPACE$$
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;
using DbBroker.Model.Interfaces;

namespace $NAMESPACE
{
    [Table(""$TABLE"", Schema = ""$SCHEMA"")]
    public class $CLASSNAME : ViewDataModel<$CLASSNAME>, IViewDataModel
    {
    $PROPERTIES
    $REFERENCES
    $CLASSES
    $TUPLE
        static $CLASSNAME()
        {
            Provider = SupportedDatabaseProviders.$PROVIDER;
        }
    }
}
";

    public const string EDM_VIEW_INTERNAL_CLASS_TEMPLATE = @"
    public class $CLASSNAME
    {
$PROPERTIES
    }";

    public const string EDM_VIEW_TUPLE_CLASS_TEMPLATE =
    @"    public class $CLASSNAMETuple : DataModel<$CLASSNAMETuple>, IViewDataModelTuple
    {
$PROPERTIES
    }
";

    public const string EDM_VIEW_PROPERTY_TEMPLATE =
@"
        [$KEYColumn(""$COLUMN_NAME""), ColumnType($$PROVIDER_DBTYPE$$)]
        public $TYPE $NAME { get; set; }
";

    public const string EDM_VIEW_PROPERTY_INDENTED_TEMPLATE =
@"
        [$KEYColumn(""$COLUMN_NAME"", ColumnType($$PROVIDER_DBTYPE$$))]
        public $TYPE $NAME { get; set; }
";


    public const string EDM_REFERENCE_VIEW_TEMPLATE =
@"
        [DataModelReference(nameof($$REFTYPENAME$$.$$REFPROPERTYNAME$$))]
        public $$REFTYPENAME$$? $$REFTYPENAME$$Ref { get; set; }
";

    public const string EDM_COLLECTION_REFERENCE_VIEW_TEMPLATE =
@"
        [DataModelCollectionReference(nameof($$REFTYPENAME$$.$$REFPROPERTYNAME$$), typeof($$REFTYPENAME$$))]
        public IEnumerable<$$REFTYPENAME$$>? $$REFTYPENAME$$Refs { get; set; }
";
}
