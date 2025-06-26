using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Deepo.Client.Web.Extension;

public static class StringExtensions
{
    public static string TrimNumberSuffixe(this string input)
    {
        return Regex.Replace(input, @"\s*\(\d+\)$", "").TrimEnd();
    }

    public static Collection<string> TrimNumberSuffixe(this Collection<string> input)
    {
        if (input != null && input.Count > 0)
        {
            for (int i = 0; i < input.Count; i++)
            {
                input[i] = TrimNumberSuffixe(input[i]);
            }
            return input;
        }
        return [];
    }
}