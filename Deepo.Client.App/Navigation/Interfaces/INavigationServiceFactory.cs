namespace Deepo.Client.App.Navigation.Interfaces
{
    public interface INavigationServiceFactory
    {
        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager, INavigationListener listener);
        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager);
    }
}