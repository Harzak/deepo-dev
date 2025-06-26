using Microsoft.Extensions.Logging;
using Ninject.Modules;
using NLog.Extensions.Logging;

namespace Deepo.Client.App.DI
{
    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<ILogger>().ToMethod(x =>
            {
                string serviceName = x?.Request?.ParentRequest?.Service.FullName ?? "Unknown";
                NLogLoggerFactory factory = new();
                return factory.CreateLogger(serviceName);
            });
        }
    }
}
