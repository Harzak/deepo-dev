using System.Collections.Specialized;
using System.Web;

namespace Deepo.Framework.Extensions;

public static class UriExtension
{
    public static Uri SetParameter(this Uri uri, string paramName, string newValue)
    {
        UriBuilder uriBuilder = new(uri);
        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query[paramName] = newValue;

        uriBuilder.Query = query.ToString();
        return uriBuilder.Uri;
    }
}
