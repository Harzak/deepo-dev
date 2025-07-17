using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Extensions;

/// <summary>
/// Extension methods for DateTime operations in the fetcher library.
/// Provides utility methods for date comparison and filtering.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Determines whether two DateTime values are in the same month and year, 
    /// </summary>
    /// <param name="date1">The first date to compare.</param>
    /// <param name="date2">The second date to compare.</param>
    /// <returns>True if the dates are in the same year and within 3 months of each other; otherwise, false.</returns>
    public static bool IsSameMonthAndYear(this DateTime date1, DateTime date2)
    {
        return date1.Year == date2.Year && date1.Month >= date2.Month;
    }
}