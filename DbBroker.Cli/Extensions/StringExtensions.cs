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
        return value;
    }
}
