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
        'f',
        "file",
        HelpText = "Provide a file path to a configuration file you want to use to synchronize. You can use multiple files at once.")]
    public IEnumerable<string> ConfigurationFiles { get; init; } = [];

    [Option(
        'd',
        "debug",
        HelpText = "Enables debug logging for the synchronization process.", Default = false)]
    public bool Debug { get; set; }
}
