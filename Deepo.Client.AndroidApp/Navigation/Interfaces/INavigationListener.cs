namespace Deepo.Client.AndroidApp.Navigation.Interfaces
{
    public interface INavigationListener
    {
        void OnNavigated(int layoutId);
        void OnNavigatedBack(int layoutId);
    }
}
