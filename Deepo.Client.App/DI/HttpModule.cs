using Deepo.Client.App.Service;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Web.Service;
using Ninject.Modules;

namespace Deepo.Client.App.DI
{
    public class HttpModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<HttpClient>().To<HttpClient>();
            base.Bind<IHttpService>().To<HttpService>();
            base.Bind<IHttpClientOption>().ToMethod((x) => new HttpClientOption(new Uri("https://deepo-api.azurewebsites.net"))
            {
                Name = "deepo.android",
                TaskID = Guid.NewGuid().ToString(),
                UserAgent = "deepo.android"
            });
        }
    }
}
