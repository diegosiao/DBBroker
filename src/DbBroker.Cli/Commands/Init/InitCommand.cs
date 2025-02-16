using DbBroker.Cli.Extensions;

namespace DbBroker.Cli.Commands.Init;

public class InitCommand
{

  private const string DbBrokerConfigFileContents =
  """
{
  "$schema": "https://raw.githubusercontent.com/diegosiao/DBBroker/refs/heads/v3.0.0/docs/json-schema/dbbroker.config.schema.json",
  "contexts": [
    {
      "namespace": "$NAMESPACE$",
      "outputDirectory": null,
      "provider": "$PROVIDER$",
      "connectionString": "$CONNECTIONSTRING$",
      "tables": [],
      "views": []
    }
  ]
}
""";

  internal static int Execute(InitOptions opts)
  {
    var dbbrokerconfigFilePath = Path.Join(Directory.GetCurrentDirectory(), "dbbroker.config.json");

    try
    {
      if (File.Exists(dbbrokerconfigFilePath) && !opts.Force)
      {
        "The 'dbbroker.config.json' file already exists. Use --force to replace it.".Error();
        return ExitCodes.CONFIG_FILE_INIT_ERROR;
      }

      File.WriteAllText(
          dbbrokerconfigFilePath,
          DbBrokerConfigFileContents
              .Replace("$NAMESPACE$", opts.Namespace)
              .Replace("$CONNECTIONSTRING$", opts.ConnectionString)
              .Replace("$PROVIDER$", opts.Provider.ToString()));

      $"Configuration file created successfully: '{dbbrokerconfigFilePath}'.".Success();
      return ExitCodes.SUCCESS;
    }
    catch (Exception ex)
    {
      Console.Error.WriteLine(ex.Message);
      Console.Error.WriteLine(ex.StackTrace);
      return ExitCodes.CONFIG_FILE_INIT_ERROR;
    }
  }
}
