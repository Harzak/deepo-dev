using Framework.Common.Utils.Time.Provider;
using Ninject.Modules;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Client.AndroidApp.DI
{
    public class UtilsModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<ITimeProvider>().To<TimeProvider>();
        }
    }
}
