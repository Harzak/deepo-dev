using Deepo.DAL.Repository.Configuration;
using Deepo.Fetcher.Viewer.Hosting;
using Deepo.Fetcher.Library.Configuration;
using NLog.Extensions.Logging;

namespace Deepo.Fetcher.Viewer;

class Program
{
    public static async Task Main(string[] args)
    {
        var host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            #region Internal dependencies
            //Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog("NLog.config");
            });

            //Host
            services.AddHostedService<ScheduledHostWorker>();
            #endregion

            #region Shared libraries dependencies
            //Deepo.DAL.Service
            services.AddDALServiceDependencies(hostContext.Configuration);

            //Deepo.Fetcher.Library
            services.AddFetcherLibraryDependencies(hostContext.Configuration);
            #endregion

        })
        .UseWindowsService()
        .Build();

        await host.RunAsync().ConfigureAwait(false);

    }
}


