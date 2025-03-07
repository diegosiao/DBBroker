using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace DbBroker.Extensions;

public static class ObjectExtensions
{
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

    public static IEnumerable<string> GetSelectColumnsFromType(this Type type, string alias = "d0.")
    {
        foreach (var prop in type.GetProperties())
        {
            var column = prop.GetCustomAttribute<ColumnAttribute>().Name;
            yield return $"{alias}{column} AS {prop.Name}";
        }
    }
}
