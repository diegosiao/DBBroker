using CommandLine;
using DbBroker.Common;

namespace DbBroker.Cli.Commands.Init;

[Verb("init", HelpText = "Initializes and outputs a 'dbbroker.config.json' with the parameters specified.")]
public class InitOptions
{
    [Option(
        'n', 
        "namespace", 
        HelpText = "The namespace the classes generated should be put in. Required.", 
        Required = true)]
    public required string Namespace { get; init; }

    [Option(
        'p', 
        "provider", 
        HelpText = "The database provider name <SqlServer|Oracle>. Default is 'SqlServer'.", 
        Default = SupportedDatabaseProviders.SqlServer)]
    public SupportedDatabaseProviders Provider { get; init; }

    [Option(
        'c', 
        "connectionString", 
        HelpText = "The database provider connection string. Required.", 
        Required = true)]
    public required string ConnectionString { get; init; }

    [Option(
        'f',
        "force",
        HelpText = "Overrides the dbbroker.config.json file if it exists. Default is false.")]
    public bool Force { get; set; }
}
