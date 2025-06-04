namespace Deepo.Fetcher.Library.Configuration.Setting;

public class HttpServicesOption
{
    public const string HttpServices = "HttpServices";

    public HttpServiceOption? Spotify { get; set; }
    public HttpServiceOption? SpotifyAuth { get; set; }
    public HttpServiceOption? Discogs { get; set; }
    public HttpServiceOption? DiscogsAuth { get; set; }
}

