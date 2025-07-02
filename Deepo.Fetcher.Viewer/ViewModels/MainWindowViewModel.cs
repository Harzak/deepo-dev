using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Wpf.Ui;

namespace Deepo.Fetcher.Viewer.ViewModels;

internal sealed class MainWindowViewModel : ViewModelBase
{
    private readonly IThemeService _themeService;

    public string ApplicationName { get; }
    public string ApplicationVersion { get; }
    public string ApplicationTitle { get; }

    public FetcherListViewModel FetcherListViewModel { get; }
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

    public async Task InitializeAsync()
    {
        await AppOverviewViewModel.InitializeAsync().ConfigureAwait(false);
    }
}