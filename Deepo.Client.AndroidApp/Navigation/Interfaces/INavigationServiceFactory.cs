namespace Deepo.Client.AndroidApp.Navigation.Interfaces
{
    public interface INavigationServiceFactory
    {
        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager, INavigationListener listener);
        public INavigationService CreateNavigationService(AndroidX.Fragment.App.FragmentManager fragmentManager);
    }
}