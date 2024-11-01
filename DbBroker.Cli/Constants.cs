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

    public const string EDM_CLASS_TEMPLATE =
@"using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace $NAMESPACE;

public class $CLASSNAME 
{
$PROPERTIES

    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);


    // WIP
    // private string[] _isNotPristineArray = [8]; // number of properties
}
";
}
