using Deepo.Fetcher.Library.Configuration.Setting;
using Framework.Web.Http.Client.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Service.Discogs;

internal class EndPointArtistReleases : MultipleResultEndpointConsumer<IEnumerable<Dto.Discogs.Release>?>
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

    protected override IEnumerable<Dto.Discogs.Release>? Parse(string text)
    {
        return JsonSerializer.Deserialize<Dto.Discogs.Releases>(text)?
            .Items;
    }
}
