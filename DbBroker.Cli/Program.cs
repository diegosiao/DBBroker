using CommandLine;
using DbBroker.Cli.Commands.Init;
using DbBroker.Cli.Commands.Sync;

namespace DbBroker.Cli;


class Program
{
    /// <summary>
    /// DbBroker tool to create and synchronize Entity Data Models
    /// </summary>
    /// <param name="args"></param>    
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
