namespace Deepo.Client.Web.Interfaces;

public interface ICatalog
{
    bool IsLoaded { get; }
    bool IsInError { get; }
    string ErrorMessage { get; }

    bool CanGoNext { get; }
    int CurrentPageIndex { get; }
    int LastPageIndex { get; }

    Task NextAsync();
    Task PreviousAsync();
    void OnPropertyChanged(Action action);
}
