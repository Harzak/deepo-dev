using Deepo.Framework.Interfaces;
using Ninject.Modules;

namespace Deepo.Client.App.DI
{
    public class UtilsModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<ITimeProvider>().To<Framework.Time.Provider.TimeProvider>();
        }
    }
}
