using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Tests.TestUtils;
public static class AsyncUtils
{
    public static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
    {
        foreach (T item in enumerable)
        {
            yield return item;
        }
        await Task.CompletedTask;
    }

    public static async IAsyncEnumerable<T> EmptyAsyncEnumerable<T>()
    {
        await Task.CompletedTask; 
        yield break;
    }
}
