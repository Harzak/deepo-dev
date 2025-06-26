using Deepo.Client.App.Fragments.Factory;
using Deepo.Client.App.Navigation.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Client.App.Navigation
{
    public class NavigationServiceFactory : INavigationServiceFactory
    {
        private readonly ILogger _logger;
        private readonly IFragmentFactory _fragmentFactory;

        public NavigationServiceFactory(ILogger logger, IFragmentFactory fragmentFactory)
        {
            _logger = logger;
            _fragmentFactory = fragmentFactory;
        }

        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager, INavigationListener listener)
            => new NavigationService(fragmentManager, listener, _fragmentFactory, _logger);

        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager)
           => new NavigationService(fragmentManager, _fragmentFactory, _logger);
    }
}
