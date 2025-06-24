using Deepo.Fetcher.Library.Configuration.Setting;
using Framework.Web.Http.Client.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Service.Discogs;
internal class EndPointSearch : SingleResultEndpointConsumer<Dto.Discogs.DtoAlbum?>
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
    public override string Delete(string id)
    {
        throw new NotImplementedException();
    }
    public override string Options()
    {
        throw new NotImplementedException();
    }
    public override string Patch(string id, string json)
    {
        throw new NotImplementedException();
    }
    public override string Post(string id, string json)
    {
        throw new NotImplementedException();
    }
    public override string Put(string id, string json)
    {
        throw new NotImplementedException();
    }
    public override string Trace()
    {
        throw new NotImplementedException();
    }
    #endregion

    protected override Dto.Discogs.DtoAlbum? Parse(string text)
    {
        return JsonSerializer.Deserialize<Dto.Discogs.AlbumSearch>(text)?
            .Results?
            .FirstOrDefault();
    }
}
