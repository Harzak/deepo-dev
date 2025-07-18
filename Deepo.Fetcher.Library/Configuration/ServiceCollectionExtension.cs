using Deepo.Fetcher.Library.Authentication;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher;
using Deepo.Fetcher.Library.Fetcher.Vinyl;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Discogs;
using Deepo.Fetcher.Library.Repositories.Spotify;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Workers.Scheduling;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Deepo.Framework.Web.Handler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Deepo.Fetcher.Library.Configuration;

/// <summary>
/// Extension methods for configuring dependency injection services for Deepo.Fetcher.Library
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds all required dependencies for the Fetcher Library to the service collection.
    /// Configures HTTP clients with rate limiting, authentication services, repositories, and factories.
    /// </summary>
    /// <param name="services">The service collection to add dependencies to.</param>
    /// <param name="configuration">The configuration instance containing application settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
    public static void AddFetcherLibraryDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        services.Configure<FetcherOptions>(configuration.GetSection("Fetcher"));
        services.Configure<HttpServicesOption>(configuration.GetSection("HttpServices"));

        services.TryAddTransient<LoggingHandler>();

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:Spotify:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new DateTimeFacade()))
                    .AddHttpMessageHandler((x)=> x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:SpotifyAuth:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new DateTimeFacade()))
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddHttpClient(configuration.GetValue<string>("HttpServices:Discogs:Name") ?? string.Empty)
                    .AddHttpMessageHandler(() => new RateLimitHandler(1, TimeSpan.FromSeconds(1), new DateTimeFacade()))
                    .AddHttpMessageHandler((x) => x.GetRequiredService<LoggingHandler>())
                    .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

        services.AddTransient<ISpotifyRepository, SpotifyRepository>();
        services.AddTransient<IDiscogRepository, DiscogRepository>();
        services.AddTransient<IAuthServiceFactory, AuthServiceFactory>();
        services.AddTransient<IDateTimeFacade, DateTimeFacade>();
        services.AddTransient<Framework.Interfaces.ITimer>(x => new Framework.Time.Timer(1000));
        services.AddTransient<IScheduler, FetcherScheduler>();
        services.AddTransient<IFetcherFactory, FetcherFactory>();
        services.AddTransient<IFetcherProvider, FetcherProvider>();
        services.AddTransient<IVinylStrategiesFactory, VinylStrategiesFactory>();
        services.AddTransient<IVinylFetchPipelineFactory, VinylFetchPipelineFactory>();
        services.AddTransient<IFetcherSchedulerDueEventFactory, FetcherSchedulerDueEventFactory>();

    }
}

