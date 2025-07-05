using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher;
using Deepo.Fetcher.Library.Fetcher.Planification;
using Deepo.Fetcher.Library.Fetcher.Vinyl;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Discogs;
using Deepo.Fetcher.Library.Repositories.Spotify;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Workers.Schedule;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Web.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Deepo.Fetcher.Library.Configuration;

public static class ServiceCollectionExtension
{
    public static void AddFetcherLibraryDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.Configure<FetcherOptions>(configuration.GetSection("Fetcher"));
        services.Configure<HttpServicesOption>(configuration.GetSection("HttpServices"));

        services.TryAddTransient<LoggingHandler>();

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:Spotify:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new Framework.Time.Provider.TimeProvider()))
                    .AddHttpMessageHandler((x)=> x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:SpotifyAuth:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new Framework.Time.Provider.TimeProvider()))
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:Discogs:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new Framework.Time.Provider.TimeProvider()))
                    .AddHttpMessageHandler((x) => x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddTransient<ISpotifyRepository, SpotifyRepository>();
        services.AddTransient<IDiscogRepository, DiscogRepository>();
        services.AddTransient<ITimeProvider, Framework.Time.Provider.TimeProvider>();
        services.AddTransient<Framework.Interfaces.ITimer>(x => new Framework.Time.Timer(1000));
        services.AddTransient<IScheduler, FetchersScheduler>();
        services.AddTransient<IFetcherFactory, FetcherFactory>();
        services.AddTransient<IFetcherProvider, FetcherProvider>();
        services.AddTransient<IPlanningFactory, PlanningFactory>();
        services.AddTransient<IVinylStrategiesFactory, VinylStrategiesFactory>();
        services.AddTransient<IVinylFetchPipelineFactory, VinylFetchPipelineFactory>();
    }
}

