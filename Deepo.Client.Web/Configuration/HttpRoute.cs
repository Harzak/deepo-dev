using System.Text;

namespace Deepo.Client.Web.Configuration;

internal static class HttpRoute
{
    public const string API_BASE_ADRESS = "https://deepo-api.azurewebsites.net";

    public const string VINYL_RELEASE_COUNT_ROUTE = "vinyl/count?market=FR";
    public const string VINYL_GENRE_ROUTE = "vinyl/genres";
    public static readonly CompositeFormat VINYL_RELEASE_ROUTE = CompositeFormat.Parse("vinyl?market=FR&offset={0}&limit={1}");
    public static readonly CompositeFormat VINYL_RELEASE_BY_ID_ROUTE = CompositeFormat.Parse("/vinyl/{0}");
}