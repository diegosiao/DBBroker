namespace DbBroker.Cli.Extensions;

public static class ConsoleExtensions
{
    public static void Success(this string console)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Error.WriteLine(console);
        Console.ResetColor();
    }

    public static void Log(this string console)
    {
        Console.WriteLine(console);
    }

    public static void Error(this string console)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine(console);
        Console.ResetColor();
    }

}
