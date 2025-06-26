using System.Text.RegularExpressions;

namespace Deepo.Client.Web.Extension;

public static class StringExtensions
{
    public static string TrimNumberSuffixe(this string input)
    {
        return Regex.Replace(input, @"\s*\(\d+\)$", "").TrimEnd();
    }
}

