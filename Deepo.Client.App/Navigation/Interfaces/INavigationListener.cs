namespace Deepo.Client.App.Navigation.Interfaces
{
    public interface INavigationListener
    {
        void OnNavigated(int layoutId);
        void OnNavigatedBack(int layoutId);
    }
}
