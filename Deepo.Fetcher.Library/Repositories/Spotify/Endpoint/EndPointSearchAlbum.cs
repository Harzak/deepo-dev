using Deepo.Fetcher.Library.Configuration;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;
using Deepo.Framework.Extensions;
using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Repositories.Spotify.Endpoint;

/// <summary>
/// Endpoint consumer for searching albums in the Spotify API.
/// Implements pagination support and provides methods for album search operations.
/// </summary>
internal class EndPointSearchAlbum : MultipleResultEndpointConsumer<IEnumerable<Dto.Spotify.DtoSpotifyAlbum>?>, IPaginableEndpointQuery
{
    private const int OFFSET_MAX_RANGE = 1000;
    private const int LIMIT_MAX_RANGE = 50;
    private const string ENDPOINT_NAME = "v1/search";

    private readonly HttpServiceOption _options;

    /// <summary>
    /// Gets or sets the market (country) code for album searches.
    /// </summary>
    public string Market { get; set; }

    internal EndPointSearchAlbum(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
        Market = Constants.DEFAULT_MARKET;
    }

    /// <summary>
    /// Constructs the GET request URL for searching albums with specified query parameters.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <returns>The relative URL path for the album search endpoint with market, type, limit, and query parameters.</returns>
    public override string Get(string query = "")
    {
        return $"{ENDPOINT_NAME}?market={Market}&type=album&limit={LIMIT_MAX_RANGE}&q={query}";
    }
    
    /// <summary>
    /// Extracts the total number of results from the API response content.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>The total number of results, or -1 if parsing fails.</returns>
    public int Total(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Total ?? -1;
    }

    /// <summary>
    /// Extracts the limit (page size) from the API response content.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>The limit value, or -1 if parsing fails.</returns>
    public int Limit(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Limit ?? -1;
    }

    /// <summary>
    /// Extracts the current offset from the API response content.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>The current offset value, or -1 if parsing fails.</returns>
    public int Offset(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Offset ?? -1;
    }

    /// <summary>
    /// Constructs the next page URL from the API response content.
    /// Handles both explicit next URLs and calculated offsets within the maximum range.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>The path and query string for the next page, or empty string if no next page exists.</returns>
    public string? Next(string content)
    {
        string? nextQuery = JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Next;

        if (string.IsNullOrEmpty(nextQuery))
        {
            Dto.Spotify.ResultSpotify<Dto.Spotify.DtoSpotifyAlbum>? result = JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result;
            if (result?.Href != null && result?.Offset != null && result.Offset < OFFSET_MAX_RANGE)
            {
                Uri nextUri = new(result.Href);
                nextUri = nextUri.SetParameter("offset", (result.Offset + LIMIT_MAX_RANGE).ToString(CultureInfo.InvariantCulture));
                nextQuery = nextUri.PathAndQuery;  
            }
        }
        return nextQuery ?? string.Empty;
    }
    
    /// <summary>
    /// Constructs the last page URL from the API response content.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>The path and query string for the last page.</returns>
    /// <exception cref="NotImplementedException">This method is not implemented.</exception>
    public string Last(string content)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Parses the JSON response content into a collection of Spotify albums.
    /// </summary>
    /// <param name="content">The JSON response content from the API.</param>
    /// <returns>A collection of Spotify albums from the search results, or null if parsing fails.</returns>
    protected override IEnumerable<Dto.Spotify.DtoSpotifyAlbum>? Parse(string content)
    {
        return JsonSerializer.Deserialize<Dto.Spotify.DtoSpotifyAlbums>(content)?.Result?.Items;
    }
}
