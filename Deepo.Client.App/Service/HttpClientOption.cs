using Deepo.Framework.Interfaces;

namespace Deepo.Client.App.Service
{
    public class HttpClientOption : IHttpClientOption
    {
        public Uri BaseAddress { get; set; }
        public string Name { get; set; }
        public string UserAgent { get; set; }
        public string TaskID { get; set; }

        public HttpClientOption(Uri uri)
        {
            BaseAddress = uri;
            Name = string.Empty;
            UserAgent = string.Empty;
            TaskID = string.Empty;
        }
    }
}
