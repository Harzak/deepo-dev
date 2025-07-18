namespace Deepo.Fetcher.Library.Configuration.Setting;

/// <summary>
/// Configuration options container for multiple HTTP services.
/// Provides centralized configuration for various external API services used by the fetcher library.
/// </summary>
public class HttpServicesOption
{
    /// <summary>
    /// The configuration section name for HTTP services settings.
    /// </summary>
    public const string HttpServices = "HttpServices";

    /// <summary>
    /// Gets or sets the HTTP service configuration for Spotify API.
    /// </summary>
    public HttpServiceOption? Spotify { get; set; }
    
    /// <summary>
    /// Gets or sets the HTTP service configuration for Spotify authentication API.
    /// </summary>
    public HttpServiceOption? SpotifyAuth { get; set; }
    
    /// <summary>
    /// Gets or sets the HTTP service configuration for Discogs API.
    /// </summary>
    public HttpServiceOption? Discogs { get; set; }
    
    /// <summary>
    /// Gets or sets the HTTP service configuration for Discogs authentication API.
    /// </summary>
    public HttpServiceOption? DiscogsAuth { get; set; }
}

