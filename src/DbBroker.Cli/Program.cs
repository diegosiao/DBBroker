using CommandLine;
using DbBroker.Cli.Commands.Init;
using DbBroker.Cli.Commands.Sync;
using DbBroker.Cli.Extensions;

namespace DbBroker.Cli;

/// <summary>
/// DBBroker CLI is a .NET tool to create and synchronize Data Models
/// </summary>
class Program
{
    static void Main(string[] args) 
    {
        $"Running DBBroker v1.0.1-alpha...".Log();

        Parser.Default.ParseArguments<InitOptions, SyncOptions>(args)
                      .MapResult(
                          (InitOptions opts) => InitOptions.Execute(opts),
                          (SyncOptions opts) => SyncCommand.Execute(opts),
                          errs => 1);
    }
}
