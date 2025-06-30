using Deepo.Fetcher.Library.Configuration.Setting;
using Framework.Web.Http.Client.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
internal class EndPointReleases : SingleResultEndpointConsumer<Dto.Discogs.DtoMaster?>
{
    private readonly HttpServiceOption _options;

    public EndPointReleases(HttpServiceOption options, ILogger logger) : base(logger)
    {
        _options = options;
    }

    public override string Get(string id)
    {
        return $"releases/{id}";
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

    protected override Dto.Discogs.DtoMaster? Parse(string text)
    {
        return JsonSerializer.Deserialize<Dto.Discogs.DtoMaster>(text);
    }
}
