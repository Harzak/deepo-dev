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

public static class ServiceCollectionExtension
{
    public static void AddDALServiceDependencies(this IServiceCollection services, IConfiguration config)
    {
        //Database
        services.AddDbContextFactory<DEEPOContext>(options => options.UseSqlServer(config.GetConnectionString("DEEPOContext")));

        //Service Dependencies
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
