using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Ninject.Modules;

namespace Deepo.Client.App.DI
{
    public class UtilsModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<IDateTimeFacade>().To<DateTimeFacade>();
        }
    }
}
