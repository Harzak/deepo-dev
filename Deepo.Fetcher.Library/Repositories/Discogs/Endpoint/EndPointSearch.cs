using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;

/// <summary>
/// Endpoint consumer for performing search operations in the Discogs API.
/// Handles requests to the enpoint: "/releases"
/// </summary>
internal sealed class EndPointSearch : MultipleResultEndpointConsumer<IEnumerable<DtoDiscogsAlbum>?>
{
    private const string ENDPOINT_NAME = "database/search";   

    private readonly HttpServiceOption _options;

    internal EndPointSearch(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }

    /// <summary>
    /// Constructs the GET request URL for performing database searches.
    /// </summary>
    /// <param name="query">The search query parameters (e.g., "artist=name&year=2024").</param>
    /// <returns>The relative URL path for the database search endpoint.</returns>
    public override string Get(string query)
    {
        return $"{ENDPOINT_NAME}?{query}";
    }
    
    /// <summary>
    /// Parses the JSON response text into a collection of Discogs albums.
    /// </summary>
    /// <param name="text">The JSON response text from the search API.</param>
    /// <returns>A collection of Discogs albums from the search results, or null if parsing fails.</returns>
    protected override IEnumerable<DtoDiscogsAlbum>? Parse(string text)
    {
        return JsonSerializer.Deserialize<AlbumSearch>(text)?.Results;
    }
}
