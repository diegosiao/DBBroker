namespace DbBroker.Cli.Extensions;

public static class ConsoleExtensions
{
    public static void Success(this string console, int linesAbove = 1)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        
        for (int i = 0; i < linesAbove; i++)
        {
            Console.WriteLine();
        }

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
