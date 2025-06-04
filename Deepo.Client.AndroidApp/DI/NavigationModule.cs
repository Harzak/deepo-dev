using Deepo.Client.AndroidApp.Fragments.Factory;
using Deepo.Client.AndroidApp.Navigation;
using Deepo.Client.AndroidApp.Navigation.Interfaces;
using Ninject.Modules;

namespace Deepo.Client.AndroidApp.DI
{
    public class NavigationModule : NinjectModule
    {
        public override void Load()
        {
            base.Bind<INavigationServiceFactory>().To<NavigationServiceFactory>();
            base.Bind<IFragmentFactory>().To<FragmentFactory>();
        }
    }
}