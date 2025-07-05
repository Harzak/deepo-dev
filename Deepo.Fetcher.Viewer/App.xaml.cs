using Deepo.DAL.Repository.Configuration;
using Deepo.Fetcher.Host.WPF.Extensions;
using Deepo.Fetcher.Viewer.Features.Listener;
using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.ViewModels;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Deepo.Framework.Time.Provider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Windows;
using Wpf.Ui;

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
                services.AddSingleton<IFetcherListenerFactory, FetcherListenerFactory>();
                services.AddTransient<ITimeProvider, TimeProvider>();
                services.AddTransient<ITimer, Timer>();

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

        MainWindowViewModel mainWindowViewModel = _host.Services.GetRequiredService<MainWindowViewModel>();
        await mainWindowViewModel.InitializeAsync().ConfigureAwait(false);
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