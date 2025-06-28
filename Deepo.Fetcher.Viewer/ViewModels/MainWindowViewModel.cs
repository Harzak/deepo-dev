using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Viewer.Interfaces;
using Framework.WPF.Behavior.ViewModel;
using System.Globalization;
using System.Reflection;
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
        IFetcherDBService fetcherDBService,
        IFetcherExecutionDBService fetcherExecutionDBService,
        IFetcherGridProviderFactory fetcherGridProviderFactory,
        IFetcherHttpRequestProviderFactory fetcherHttpRequestProviderFactory)
    {
        _themeService = themeService;

        ApplicationVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.0.0";
        ApplicationName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
        ApplicationTitle = string.Format(CultureInfo.CurrentCulture, "{0} - {1}", ApplicationName, ApplicationVersion);

        FetcherListViewModel = new FetcherListViewModel(fetcherDBService, fetcherGridProviderFactory, fetcherHttpRequestProviderFactory);
        AppOverviewViewModel = new AppOverviewViewModel(fetcherExecutionDBService);
    }
}