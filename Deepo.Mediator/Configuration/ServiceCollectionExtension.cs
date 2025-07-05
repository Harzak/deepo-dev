using Deepo.DAL.Repository.Configuration;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deepo.Mediator.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMediatRDependencies(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        services.AddDALServiceDependencies(config);
        services.AddTransient<ITimeProvider, Framework.Time.Provider.TimeProvider>();
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
        return services;
    }
}
