using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Feature.Author;
using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.DAL.Service.Feature.ReleaseAlbum;
using Deepo.DAL.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deepo.DAL.Service.Configuration;

public static class ServiceCollectionExtension
{
    public static void AddDALServiceDependencies(this IServiceCollection services, IConfiguration config)
    {
        //Database
        services.AddDbContext<DEEPOContext>(options => options.UseSqlServer(config.GetConnectionString("DEEPOContext")), ServiceLifetime.Singleton);

        //Service Dependencies
        services.AddTransient<IAuthorDBService, AuthorDBService>();
        services.AddTransient<IFetcherExecutionDBService, FetcherExecutionDBService>();
        services.AddTransient<IFetcherDBService, FetcherDBService>();
        services.AddTransient<IPlanificationDBService, PlanificationDBService>();
        services.AddTransient<IReleaseAlbumDBService, ReleaseAlbumDBService>();
    }
}
