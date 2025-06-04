using Deepo.DAL.Service.Configuration;
using Framework.Common.Utils.Time.Provider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Mediator.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddMediatRDependencies(this IServiceCollection services, IConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(config, nameof(config));

        services.AddDALServiceDependencies(config);
        services.AddTransient<ITimeProvider, TimeProvider>();
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly));
        return services;
    }
}
