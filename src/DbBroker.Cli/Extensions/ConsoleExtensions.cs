using System.Diagnostics;

namespace DbBroker.Cli.Extensions;

public static class ConsoleExtensions
{
    public static void Success(this string console, string? contextNamespace = null, int linesAbove = 1)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        for (int i = 0; i < linesAbove; i++)
        {
            Console.WriteLine();
        }

        if (!string.IsNullOrEmpty(contextNamespace))
        {
            Console.Write($"{contextNamespace} | ");
        }
        Console.WriteLine(console);
        Console.ResetColor();
    }

    public static void Warning(this string console, string? contextNamespace = null, int linesAbove = 1)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        for (int i = 0; i < linesAbove; i++)
        {
            Console.WriteLine();
        }

        if (!string.IsNullOrEmpty(contextNamespace))
        {
            Console.Write($"{contextNamespace} | ");
        }
        Console.WriteLine(console);
        Console.ResetColor();
    }

    public static void Log(this string console, string? contextNamespace = null)
    {
        if (!string.IsNullOrEmpty(contextNamespace))
        {
            Console.Write($"{contextNamespace} | ");
        }
        Console.WriteLine(console);
    }

    public static void Error(this string message, string? contextNamespace = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;

        if (!string.IsNullOrEmpty(contextNamespace))
        {
            Console.Error.Write($"{contextNamespace} | ");
        }
        Console.Error.WriteLine(message);
        Console.ResetColor();
    }

    public static void Debug(this string message, string? contextNamespace = null)
    {
        if (!Debugger.IsAttached)
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;

        if (!string.IsNullOrEmpty(contextNamespace))
        {
            Console.Write($"{contextNamespace} | ");
        }
        Console.WriteLine(message);
        Console.ResetColor();
    }

}
