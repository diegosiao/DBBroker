using CommandLine;

namespace DbBroker.Cli.Commands.Sync;

[Verb("sync", HelpText = "Synchronizes your Entity Data Models with the respective database")]
public class SyncOptions
{
    /// <summary>
    /// The specific database you want to update
    /// </summary>
    [Option('d', "database", HelpText="The specific database you want to update")]
    public string? Context { get; init; }

    [Option('c', "configFilesDirectory", HelpText="Provide a file path directory to one or more configuration file you want to use to synchronize. The file name need to start with 'dbbroker.config*'.")]
    public string? ConfigFilesDirectory { get; init; }
}
