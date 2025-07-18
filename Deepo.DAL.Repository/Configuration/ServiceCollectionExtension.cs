using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Author;
using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Feature.Genre;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deepo.DAL.Repository.Configuration;

/// <summary>
/// Provides extension methods for configuring Data Access Layer services in the dependency injection container.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Registers database context and repository dependencies required for data access operations.
    /// </summary>
    public static void AddDALServiceDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContextFactory<DEEPOContext>(options => options.UseSqlServer(config.GetConnectionString("DEEPOContext")));

        services.AddSingleton<IAuthorRepository, AuthorRepository>();
        services.AddSingleton<IFetcherExecutionRepository, FetcherExecutionRepository>();
        services.AddSingleton<IFetcherRepository, FetcherRepository>();
        services.AddSingleton<ISchedulerRepository, SchedulerRepository>();
        services.AddSingleton<IReleaseAlbumRepository, ReleaseAlbumRepository>();
        services.AddSingleton<IGenreAlbumRepository, GenreAlbumRepository>();
        services.AddSingleton<IFetcherHttpRequestRepository, FetcherHttpRequestRepository>();
        services.AddSingleton<IReleaseHistoryRepository, ReleaseHistoryRepository>();
    }
}
