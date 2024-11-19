using System;

namespace DbBroker.Cli.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Transforms an object name like 'MY_TABLE' into MyTable
    /// </summary>
    /// <param name="value"></param>
    public static string ToCamelCase(this string value)
    {
        if (value?.IndexOf('_') != -1)
        {
            var words = value!.Split('_');
            return string.Join(string.Empty, words.Select(w => $"{w.ToUpperInvariant()[0]}{w.ToLowerInvariant()[1..]}"));
        }

        return value?.All(char.IsUpper) ?? false
            ? $"{value?.ToUpperInvariant()[0]}{value?.ToLowerInvariant()[1..]}"
            : value ?? string.Empty;
    }
}
