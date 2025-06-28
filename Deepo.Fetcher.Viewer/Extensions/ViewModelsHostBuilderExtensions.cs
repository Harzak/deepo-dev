using Deepo.Fetcher.Viewer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Deepo.Fetcher.Host.WPF.Extensions
{
    internal static class ViewModelsHostBuilderExtensions
    {
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
}
