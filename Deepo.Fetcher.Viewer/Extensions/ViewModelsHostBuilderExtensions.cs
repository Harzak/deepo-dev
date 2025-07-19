using Deepo.Fetcher.Viewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Deepo.Fetcher.Viewer.Extensions;

/// <summary>
/// Provides extension methods for configuring view models in the dependency injection container.
/// </summary>
internal static class ViewModelsHostBuilderExtensions
{
    /// <summary>
    /// Configures view model services for the host builder.
    /// </summary>
    public static IHostBuilder BuildViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<FetcherListViewModel>();
            services.AddSingleton<Func<FetcherListViewModel>>((s) => () => s.GetRequiredService<FetcherListViewModel>());
            services.AddSingleton<MainWindowViewModel>();
        });

        return hostBuilder;
    }
}
