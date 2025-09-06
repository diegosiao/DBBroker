using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace DbBroker.Extensions;

/// <summary>
/// Provides extension methods for objects
/// </summary>
public static class ObjectExtensions
{
    /// <summary>
    /// Gets the value of the property marked with [Key] attribute
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static object GetKeyValue(this object obj)
    {
        if (obj is null)
        {
            return null;
        }

        var keyProperty = obj
            .GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttribute<KeyAttribute>() != null)
            .FirstOrDefault();

        return keyProperty?.GetValue(obj);
    }

    /// <summary>
    /// Gets the list of columns for a given type, using the [Column] attribute to get the column name.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="alias"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetSelectColumnsFromType(this Type type, string alias = "d0.")
    {
        foreach (var prop in type.GetProperties())
        {
            var column = prop.GetCustomAttribute<ColumnAttribute>().Name;
            yield return $"{alias}{column} AS {prop.Name}";
        }
    }
}
