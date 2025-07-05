using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Framework.Extensions;

public static class StringExtension
{
    public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }

    public static string Truncate(this string str, int maxLength, string? replacement = "")
    {
        ArgumentNullException.ThrowIfNull(str);
        if (str.Length <= maxLength || Math.Max(0, maxLength) < (replacement?.Length ?? 0))
        {
            return str;
        }
        else
        {
            int adjustedLength = maxLength - (replacement?.Length ?? 0);
            return string.Concat(str.AsSpan(0, adjustedLength), replacement);
        }
    }
}