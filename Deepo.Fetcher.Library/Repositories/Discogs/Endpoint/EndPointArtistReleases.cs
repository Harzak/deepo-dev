using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Framework.Web.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;

internal class EndPointArtistReleases : MultipleResultEndpointConsumer<IEnumerable<DtoDiscogsRelease>?>
{
    private readonly HttpServiceOption _options;

    public EndPointArtistReleases(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }

    public override string Get(string query)
    {
        return $"artists/{query}";
    }

    public override string Options()
    {
        throw new NotImplementedException();
    }

    public override string Trace()
    {
        throw new NotImplementedException();
    }

    protected override IEnumerable<DtoDiscogsRelease>? Parse(string text)
    {
        return JsonSerializer.Deserialize<DtoDiscogsReleaseList>(text)?
            .Items;
    }
}
