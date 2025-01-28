using DbBroker.Cli.Extensions;
using DbBroker.Cli.Services;
using DbBroker.Common.Model;
using Newtonsoft.Json;

namespace DbBroker.Cli.Commands.Sync;

public class SyncCommand
{
    internal static Dictionary<string, int> Results = [];

    public static int Execute(SyncOptions options)
    {
        "Synchronizing...".Log();
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
                contexts.AddRange(config!.Contexts!);
            }
        }

        var tasks = contexts
            .Select(x => new CSharpClassGenerator().GenerateAsync(x));

        Task.WhenAll(tasks).Wait();

        if (Results.Sum(x => x.Value) == 0)
        {
            "Data Models synchronized.".Success();
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
