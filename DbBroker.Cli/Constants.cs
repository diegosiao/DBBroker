namespace DbBroker.Cli;

public static class Constants
{
    public const string CONFIG_FILENAME = "dbbroker.config.json";

    public const string EDM_PROPERTY_TEMPLATE = "    public $TYPE $NAME { get; set; }";

    public const string EDM_CLASS_TEMPLATE = 
@"using System;

namespace $NAMESPACE;

public class $CLASSNAME 
{
$PROPERTIES
}
";
}
