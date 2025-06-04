using Deepo.Fetcher.Library.Configuration.Setting;
using Framework.Web.Http.Client.Endpoint;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Deepo.Fetcher.Library.Service.Discogs;
internal class EndPointMasters : SingleResultEndpointConsumer<Dto.Discogs.Master?>
{
    private readonly HttpServiceOption _options;

    public EndPointMasters(HttpServiceOption options, ILogger logger) : base(logger)
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

    protected override Dto.Discogs.Master? Parse(string text)
    {
        var e = JsonSerializer.Deserialize<Dto.Discogs.Master>(text);
        return e;
    }
}
