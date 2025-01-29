using System;
using CommandLine;
using DbBroker.Cli.Extensions;
using DbBroker.Common;
using DbBroker.Common.Model;

namespace DbBroker.Cli.Commands.Init;

[Verb("init", HelpText = "Initializes the 'dbbroker.config.json' file")]
public class InitOptions
{
    [Option(
        'v', 
        "vendor", 
        HelpText = "Database vendor <SqlServer|Oracle>", 
        Default = SupportedDatabaseProviders.SqlServer)]
    public SupportedDatabaseProviders Vendor { get; init; }

    [Option('c', "connectionString", HelpText = "Database vendor connection string", Required = true)]
    public required string ConnectionString { get; init; }

    internal static int Execute(InitOptions opts)
    {
        $"Initializing '{opts.Vendor}'".Log();
        return 0;
    }
}
