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
        $"Running DBBroker CLI v1.0.1-beta...".Log();

        Parser.Default.ParseArguments<InitOptions, SyncOptions>(args)
                      .MapResult(
                          (InitOptions opts) => InitCommand.Execute(opts),
                          (SyncOptions opts) => SyncCommand.Execute(opts),
                          errs => 1);
    }
}
