using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher;
using Deepo.Fetcher.Library.Fetcher.Planification;
using Deepo.Fetcher.Library.Fetcher.Vinyl;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Discogs;
using Deepo.Fetcher.Library.Repositories.Spotify;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Framework.Common.Utils.Time.Provider;
using Framework.Common.Worker.Schedule;
using Framework.Common.Worker.Schedule.Planification;
using Framework.Web.Http.Client.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ITimer = Framework.Common.Utils.Time.ITimer;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

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
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(2), new TimeProvider()))
                    .AddHttpMessageHandler((x)=> x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:SpotifyAuth:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(2), new TimeProvider()))
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:Discogs:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(2), new TimeProvider()))
                    .AddHttpMessageHandler((x) => x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddTransient<ISpotifyRepository, SpotifyRepository>();
        services.AddTransient<IDiscogRepository, DiscogRepository>();
        services.AddTransient<ITimeProvider, TimeProvider>();
        services.AddTransient<ITimer>(x => new Framework.Common.Utils.Time.Timer(1000));
        services.AddTransient<IScheduler, FetchersScheduler>();
        services.AddTransient<IFetcherFactory, FetcherFactory>();
        services.AddTransient<IFetcherProvider, FetcherProvider>();
        services.AddTransient<IPlanningFactory, PlanningFactory>();
        services.AddTransient<IVinylStrategiesFactory, VinylStrategiesFactory>();
        services.AddTransient<IVinylFetchPipelineFactory, VinylFetchPipelineFactory>();
    }
}

