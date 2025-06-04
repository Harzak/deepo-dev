using Deepo.Client.AndroidApp.Service;
using Framework.Web.Http.Client.Service;
using Ninject.Modules;

namespace Deepo.Client.AndroidApp.DI
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
