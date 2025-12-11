namespace DbBroker.Cli.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Transforms an object name like 'LAST_NAME' or 'last_name' into LastName
    /// </summary>
    /// <param name="value"></param>
    public static string ToCamelCase(this string value)
    {
        if (value?.IndexOf('_') != -1)
        {
            var words = value!.Split('_');
            return string.Join(string.Empty, words.Select(w => $"{w.ToUpperInvariant()[0]}{w.ToLowerInvariant()[1..]}"));
        }

        return $"{(value ?? string.Empty).ToUpperInvariant()[0]}{(value ?? string.Empty).ToLowerInvariant()[1..]}";
    }
}
