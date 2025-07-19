using Deepo.DAL.Repository.Configuration;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deepo.Mediator.Configuration;

/// <summary>
/// Provides extension methods for configuring MediatR and related dependencies in the service collection.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds MediatR dependencies including handlers, repositories, and time services to the service collection.
    /// </summary>
    public static IServiceCollection AddMediatRDependencies(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        services.AddDALServiceDependencies(config);
        services.AddTransient<IDateTimeFacade, DateTimeFacade>();
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
        return services;
    }
}
