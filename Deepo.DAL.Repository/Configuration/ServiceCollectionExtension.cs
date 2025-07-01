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
        services.AddDbContext<DEEPOContext>(options => options.UseSqlServer(config.GetConnectionString("DEEPOContext")), ServiceLifetime.Singleton);

        //Service Dependencies
        services.AddTransient<IAuthorRepository, AuthorRepository>();
        services.AddTransient<IFetcherExecutionRepository, FetcherExecutionRepository>();
        services.AddTransient<IFetcherRepository, FetcherRepository>();
        services.AddTransient<IPlanificationRepository, PlanificationRepository>();
        services.AddTransient<IReleaseAlbumRepository, ReleaseAlbumRepository>();
        services.AddTransient<IGenreAlbumRepository, GenreAlbumRepository>();
        services.AddTransient<IFetcherHttpRequestRepository, FetcherHttpRequestRepository>();
        services.AddTransient<IReleaseHistoryRepository, ReleaseHistoryRepository>();
    }
}
