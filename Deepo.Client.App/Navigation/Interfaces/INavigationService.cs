using Android.Views;

namespace Deepo.Client.App.Navigation.Interfaces
{
    public interface INavigationService : IDisposable
    {
        bool NavigateTo(IMenuItem item);
        bool NavigateToHome();
        bool NavigateToNewReleases();
        bool NavigateToSettings();
        bool NavigateBack(Activity activity);
    }
}
