using Framework.Web.Http.Client.Service;
using System.Reflection;

namespace Deepo.Fetcher.Library.Configuration.Setting;

public class HttpServiceOption : IHttpClientOption
{
    public Uri BaseAddress { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public string UserAgent { get; set; } = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
    public string ProviderCode { get; set; } = string.Empty;
    public string ConsumerKey { get; set; } = string.Empty;
    public string ConsumerSecret { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string TaskID { get; set; } = string.Empty;

    public HttpServiceOption()
    {

    }
}

