using CommandLine;
using DbBroker.Cli.Commands.Init;
using DbBroker.Cli.Commands.Sync;
using DbBroker.Cli.Extensions;

namespace DbBroker.Cli;

/// <summary>
/// DbBroker .NET tool to create and synchronize Entity Data Models
/// </summary>
class Program
{
    static void Main(string[] args) 
    {
        $"Running DbBroker...".Log();
        Parser.Default.ParseArguments<InitOptions, SyncOptions>(args)
                      .MapResult(
                          (InitOptions opts) => InitOptions.Execute(opts),
                          (SyncOptions opts) => SyncCommand.Execute(opts),
                          errs => 1);
    }
}
