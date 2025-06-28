using Deepo.DAL.Service.Configuration;
using Deepo.Fetcher.Host.WPF.Extensions;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.Features.FetcherGrid.Provider;
using Deepo.Fetcher.Viewer.ViewModels;
using Framework.Common.Utils.Time;
using Framework.Common.Utils.Time.Provider;
using Framework.WPF.Behavior.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Windows;
using Wpf.Ui;
using Deepo.Fetcher.Viewer.Features.FetcherHttpRequest.Provider;

namespace Deepo.Fetcher.Host.WPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .BuildViewModels()
            .ConfigureServices((hostContext, services) =>
            {
                #region Internal dependencies

                //Logging
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddNLog("NLog.config");
                });

                services.AddSingleton<IThemeService, ThemeService>();
                services.AddSingleton<ITaskBarService, TaskBarService>();
                services.AddSingleton<ISnackbarService, SnackbarService>();
                services.AddSingleton<IContentDialogService, ContentDialogService>();
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<IFetcherGridProviderFactory, FetcherGridProviderFactory>();
                services.AddSingleton<IFetcherHttpRequestProviderFactory, FetcherHttpRequestProviderFactory>();
                services.AddTransient<ITimeProvider, TimeProvider>();
                services.AddTransient<ITimer, Timer>();
                services.AddSingleton<NavigationState>();

                services.AddSingleton(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainWindowViewModel>()
                });

                #endregion

                #region Shared libraries dependencies
                //Deepo.DAL.Service
                services.AddDALServiceDependencies(hostContext.Configuration);

                #endregion
            })
            .Build();
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await _host.StartAsync().ConfigureAwait(false);

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync().ConfigureAwait(false);
        _host.Dispose();
    }
}