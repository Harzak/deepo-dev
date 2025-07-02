using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Extensions;

public static class DateTimeExtensions
{
    public static bool IsSameMonthAndYear(this DateTime date1, DateTime date2)
    {
        return date1.Year == date2.Year && date1.Month >= date2.Month - 3;
    }
}