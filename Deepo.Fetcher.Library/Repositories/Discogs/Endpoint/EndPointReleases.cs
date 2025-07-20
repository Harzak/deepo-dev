using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;

/// <summary>
/// Endpoint consumer for retrieving specific release information from the Discogs API.
/// Handles requests to the enpoint: "/releases"
/// </summary>
internal sealed class EndPointReleases : SingleResultEndpointConsumer<DtoDiscogsRelease?>
{
    private const string ENDPOINT_NAME = "releases"; 

    private readonly HttpServiceOption _options;

    public EndPointReleases(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }


    /// <summary>
    /// Constructs the GET request URL for retrieving a specific release.
    /// </summary>
    /// <param name="id">The release ID to retrieve.</param>
    /// <returns>The relative URL path for the releases endpoint.</returns>
    public override string Get(string id)
    {
        return $"{ENDPOINT_NAME}/{id}";
    }
    
    /// <summary>
    /// Parses the JSON response text into a Discogs release object.
    /// </summary>
    /// <param name="text">The JSON response text from the API.</param>
    /// <returns>A Discogs release object, or null if parsing fails.</returns>
    protected override DtoDiscogsRelease? Parse(string text)
    {
        return JsonSerializer.Deserialize<DtoDiscogsRelease>(text);
    }
}
