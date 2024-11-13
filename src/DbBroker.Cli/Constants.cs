namespace DbBroker.Cli;

public static class Constants
{
    public const string CONFIG_FILENAME = "dbbroker.config.json";

    public const string EDM_KEY_ATTRIBUTE_TEMPLATE = @"    [Key]";

    public const string EDM_PROPERTY_TEMPLATE =
@"
    private $TYPE _$NAME;

    [$KEYColumn(name: ""$COLUMN_NAME"")]
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

    public const string EDM_CLASS_TEMPLATE =
@"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbBroker.Attributes;
using DbBroker.Common;
using DbBroker.Model;

namespace $NAMESPACE;

[Table(name: ""$TABLE"", Schema = ""$SCHEMA"")]
public class $CLASSNAME : DataModel<$CLASSNAME>
{
$PROPERTIES
$REFERENCES

    static $CLASSNAME()
    {
        Provider = SupportedDatabaseProviders.$PROVIDER;
        ISqlInsertTemplateTypeFullName = ""$ISQLINSERTTEMPLATETYPEFULLNAME"";
    }
}
";
}
