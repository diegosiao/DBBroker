using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DbBroker.Extensions;

/// <summary>
/// Provides LINQ extension methods for .NET Standard 2.0
/// </summary>
public static class NetStandard2LinqExtensions
{
    /// <summary>
    /// https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/Skip.cs
    /// </summary>
    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
    {
        var queue = new Queue<T>();

        using (var e = source.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (queue.Count == count)
                {
                    do
                    {
                        yield return queue.Dequeue();
                        queue.Enqueue(e.Current);
                    } while (e.MoveNext());
                }
                else
                {
                    queue.Enqueue(e.Current);
                }
            }
        }
    }

    /// <summary>
    /// https://github.com/dotnet/corefx/blob/master/src/System.Linq/src/System/Linq/Take.cs
    /// </summary>
    public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        return count <= 0 ?
            Array.Empty<TSource>() :
            TakeLastEnumerableFactory(source, count);
    }

    private static IEnumerable<TSource> TakeLastEnumerableFactory<TSource>(IEnumerable<TSource> source, int count) =>
        TakeLastIterator<TSource>(source, count);

    private static IEnumerable<TSource> TakeLastIterator<TSource>(IEnumerable<TSource> source, int count)
    {
        Debug.Assert(source != null);
        Debug.Assert(count > 0);

        Queue<TSource> queue;
        using (IEnumerator<TSource> e = source.GetEnumerator())
        {
            if (!e.MoveNext())
            {
                yield break;
            }

            queue = new Queue<TSource>();
            queue.Enqueue(e.Current);

            while (e.MoveNext())
            {
                if (queue.Count < count)
                {
                    queue.Enqueue(e.Current);
                }
                else
                {
                    do
                    {
                        queue.Dequeue();
                        queue.Enqueue(e.Current);
                    }
                    while (e.MoveNext());
                    break;
                }
            }
        }

        Debug.Assert(queue.Count <= count);
        do
        {
            yield return queue.Dequeue();
        }
        while (queue.Count > 0);
    }
}
