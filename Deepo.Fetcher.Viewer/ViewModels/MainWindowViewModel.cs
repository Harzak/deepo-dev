using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Wpf.Ui;

namespace Deepo.Fetcher.Viewer.ViewModels;

/// <summary>
/// Serves as the primary view model for the main application window, coordinating sub-view models and application-level properties.
/// </summary>
internal sealed class MainWindowViewModel : BaseViewModel
{
    private readonly IThemeService _themeService;

    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    public string ApplicationName { get; }
    
    /// <summary>
    /// Gets the version of the application.
    /// </summary>
    public string ApplicationVersion { get; }
    
    /// <summary>
    /// Gets the formatted title combining application name and version.
    /// </summary>
    public string ApplicationTitle { get; }

    /// <summary>
    /// Gets the view model for managing the fetcher list interface.
    /// </summary>
    public FetcherListViewModel FetcherListViewModel { get; }
    
    /// <summary>
    /// Gets the view model for displaying application overview information.
    /// </summary>
    public AppOverviewViewModel AppOverviewViewModel { get; }

    public MainWindowViewModel(IThemeService themeService,
        IFetcherRepository fetcherRepository,
        IFetcherExecutionRepository fetcherExecutionRepository,
        IFetcherListenerFactory fetcherListenerFactory)
    {
        _themeService = themeService;

        ApplicationVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.0.0";
        ApplicationName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
        ApplicationTitle = string.Format(CultureInfo.CurrentCulture, "{0} - {1}", ApplicationName, ApplicationVersion);

        IFetcherListener listener = fetcherListenerFactory.CreateFetcherListener();
        listener.StartListener();

        FetcherListViewModel = new FetcherListViewModel(fetcherRepository, listener);
        AppOverviewViewModel = new AppOverviewViewModel(fetcherExecutionRepository, listener);
    }

    /// <summary>
    /// Asynchronously initializes the main window and its sub-components.
    /// </summary>
    public async Task InitializeAsync()
    {
        await AppOverviewViewModel.InitializeAsync().ConfigureAwait(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            FetcherListViewModel.Dispose();
            AppOverviewViewModel.Dispose();
        }
        base.Dispose(disposing);
    }
}