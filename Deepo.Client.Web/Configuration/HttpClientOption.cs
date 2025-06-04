using Framework.Web.Http.Client.Service;
using System.Globalization;

namespace Deepo.Client.Web.Configuration;

public class HttpClientOption : IHttpClientOption
{
    public Uri BaseAddress { get; set; }
    public string Name { get; set; }
    public string UserAgent { get; set; }
    public string TaskID { get; set; }

    public HttpClientOption()
    {
        BaseAddress = default!;
        Name = string.Empty;
        UserAgent = string.Empty;
        TaskID = Guid.Empty.ToString();
    }
}

