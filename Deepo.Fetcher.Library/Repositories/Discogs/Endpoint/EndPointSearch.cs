using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Framework.Web.Http.Client.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
internal class EndPointSearch : MultipleResultEndpointConsumer<IEnumerable<DtoDiscogsAlbum>?>
{
    private readonly HttpServiceOption _options;

    internal EndPointSearch(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }

    #region HTTP Methods
    public override string Get(string query)
    {
        return $"database/search?{query}";
    }
    public override string Options()
    {
        throw new NotImplementedException();
    }
    public override string Trace()
    {
        throw new NotImplementedException();
    }
    #endregion

    protected override IEnumerable<DtoDiscogsAlbum>? Parse(string text)
    {
        return JsonSerializer.Deserialize<AlbumSearch>(text)?.Results;
    }
}
