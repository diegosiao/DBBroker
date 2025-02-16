using CommandLine;

namespace DbBroker.Cli.Commands.Sync;

[Verb("sync", HelpText = "Synchronizes your Entity Data Models with the respective database")]
public class SyncOptions
{
    [Option(
        'n', 
        "namespace", 
        HelpText = "The specific context namespace you want to update. You can use the namespace or context name.")]
    public string? Context { get; init; }

    [Option(
        'c', 
        "configFilesDirectory", 
        HelpText = "Provide a file path directory to one or more configuration file you want to use to synchronize. File names to starting with 'dbbroker.config*' will be processed.")]
    public string? ConfigFilesDirectory { get; init; }
}
