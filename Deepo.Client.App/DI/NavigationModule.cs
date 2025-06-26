using Deepo.Client.App.Fragments.Factory;
using Deepo.Client.App.Navigation;
using Deepo.Client.App.Navigation.Interfaces;
using Ninject.Modules;

namespace Deepo.Client.App.DI
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