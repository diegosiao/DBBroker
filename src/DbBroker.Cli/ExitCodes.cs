namespace DbBroker.Cli;

public static class ExitCodes
{
    public const int SUCCESS = 0;

    public const int CONFIG_FILE_NOTFOUND = 1;

    public const int CONFIG_FILE_INIT_ERROR = 2;

    public const int CONFIG_INVALID = 3;

    public const int NO_CONTEXT_LOADED_ERROR = 4;

    public const int CONTEXT_CONNECTION_ERROR = 5;

    public static readonly Dictionary<int, string> Messages = new()
    {
        { SUCCESS, "Success" },
        { CONFIG_FILE_NOTFOUND, "Configuration file not found" },
        { CONFIG_FILE_INIT_ERROR, "Error creating configuration file" },
        { CONFIG_INVALID, "Invalid configuration found" },
        { NO_CONTEXT_LOADED_ERROR, "No database context loaded" },
        { CONTEXT_CONNECTION_ERROR, "Error connecting to database context" }
    };
}