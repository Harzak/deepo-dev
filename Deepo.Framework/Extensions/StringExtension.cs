using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Framework.Extensions;

/// <summary>
/// Provides extension methods for string manipulation and validation operations.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Truncates the string to the specified maximum length and optionally appends a replacement string.
    /// </summary>
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