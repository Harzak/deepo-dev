using System.Collections.Specialized;
using System.Web;

namespace Deepo.Framework.Extensions;

/// <summary>
/// Provides extension methods for URI manipulation and query parameter operations.
/// </summary>
public static class UriExtension
{
    /// <summary>
    /// Sets or updates a query parameter in the URI with the specified name and value.
    /// </summary>
    public static Uri SetParameter(this Uri uri, string paramName, string newValue)
    {
        UriBuilder uriBuilder = new(uri);
        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query[paramName] = newValue;

        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri;
    }
}
