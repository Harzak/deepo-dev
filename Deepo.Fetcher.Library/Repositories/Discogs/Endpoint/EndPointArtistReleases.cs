using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;

/// <summary>
/// Endpoint consumer for retrieving artist releases from the Discogs API.
/// Handles requests to the enpoint: "/artists"
/// </summary>
internal class EndPointArtistReleases : MultipleResultEndpointConsumer<IEnumerable<DtoDiscogsRelease>?>
{
    private const string ENDPOINT_NAME = "artists";    

    private readonly HttpServiceOption _options;

    public EndPointArtistReleases(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }

    /// <summary>
    /// Constructs the GET request URL for retrieving artist releases.
    /// </summary>
    /// <param name="query">The artist ID to retrieve releases for.</param>
    /// <returns>The relative URL path for the artist releases endpoint.</returns>
    public override string Get(string query)
    {
        return $"{ENDPOINT_NAME}/{query}";
    }

    /// <summary>
    /// Parses the JSON response text into a collection of Discogs releases.
    /// </summary>
    /// <param name="text">The JSON response text from the API.</param>
    /// <returns>A collection of Discogs releases, or null if parsing fails.</returns>
    protected override IEnumerable<DtoDiscogsRelease>? Parse(string text)
    {
        return JsonSerializer.Deserialize<DtoDiscogsReleaseList>(text)?
            .Items;
    }
}
