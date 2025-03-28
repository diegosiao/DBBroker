using DbBroker.Cli.Extensions;
using DbBroker.Cli.Services;
using DbBroker.Common.Model;
using Newtonsoft.Json;

namespace DbBroker.Cli.Commands.Sync;

public class SyncCommand
{
    internal static Dictionary<string, int> Results = [];

    internal static SyncOptions Options { get; private set; } = new SyncOptions();

    public static int Execute(SyncOptions options)
    {
        Options = options;

        var startTime = DateTime.Now;
        $"Synchronizing... Started at {startTime}.".Log();
        var configFilesDirectory = options.ConfigFilesDirectory ?? Directory.GetCurrentDirectory();
        var configFiles = Directory.EnumerateFiles(configFilesDirectory, "dbbroker.config.*");

        if (!configFiles.Any())
        {
            $"No configuration file was found at '{configFilesDirectory}'.".Error();
            $"Run 'dbbroker init' at your *.csproj file level to create one.".Error();
            return ExitCodes.CONFIG_FILE_NOTFOUND;
        }

        $"Loading {configFiles.Count()} configuration(s) file(s):".Log();
        List<DbBrokerConfigContext> contexts = [];
        foreach (var configFile in configFiles)
        {
            $"- {configFile}".Log();
            if (TryParseConfig(configFile, out DbBrokerConfig? config))
            {
                if (config?.Database is not null)
                {
                    foreach (var context in config.Contexts)
                    {
                        context.Provider ??= config.Database.Provider;
                        context.ConnectionString ??= config.Database.ConnectionString;
                    }
                }

                contexts.AddRange(config!.Contexts!);
            }
        }

        "".Log(); // empty line

        var tasks = contexts
            .Select(x => new CSharpClassGenerator().GenerateAsync(x));

        Task.WhenAll(tasks).Wait();

        if (Results.Sum(x => x.Value) == 0)
        {
            $"Synchronization finished at '{DateTime.Now}' ({(DateTime.Now - startTime).TotalSeconds:N2} seconds).".Success();
            return ExitCodes.SUCCESS;
        }

        "There is an error synchronizing the Data Models.".Error();
        "Namespaces with error(s): ".Error();
        foreach (var result in Results)
        {
            $"- {result.Key}: Code {result.Value}".Error();
        }
        return 1;
    }

    private static bool TryParseConfig(string configFile, out DbBrokerConfig? config)
    {
        config = null;
        try
        {
            var json = File.ReadAllText(configFile);
            config = JsonConvert.DeserializeObject<DbBrokerConfig>(json);
            return true;
        }
        catch (Exception ex)
        {
            $"    > Error deserializing '{configFile}': {ex.Message}".Error();
            return false;
        }
    }
}
