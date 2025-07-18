using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher.Movie;
using Deepo.Fetcher.Library.Fetcher.Vinyl;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Fetcher;

/// <summary>
/// Creates specific types of fetcher worker instances.
/// </summary>
internal class FetcherFactory : IFetcherFactory
{
    private readonly ILogger _logger;
    private readonly IOptions<FetcherOptions> _fetcherOptions;
    private readonly IVinylFetchPipelineFactory _vinylFetchPipelineFactory;

    public FetcherFactory(IOptions<FetcherOptions> fetcherOptions,
        IVinylFetchPipelineFactory vinylFetchPipelineFactory,
        ILogger<FetcherFactory> logger)
    {
        _logger = logger;
        _fetcherOptions = fetcherOptions;
        _vinylFetchPipelineFactory = vinylFetchPipelineFactory;
    }

    /// <summary>
    /// Creates a fetcher worker instance based on the provided code.
    /// </summary>
    /// <param name="code">The code identifying the type of fetcher to create.</param>
    /// <returns>A worker instance corresponding to the specified code.</returns>
    public IWorker CreateFetcher(string code)
    {
        switch (code)
        {
            case "FETCHER_VINYL":
                return new FetcherVinyl(_vinylFetchPipelineFactory, _logger)
                {
                    ID = Guid.Parse(_fetcherOptions.Value.FetcherVinyleId),
                    Name  = _fetcherOptions.Value.FetcherVinyleName
                };
            case "FETCHER_DEBUG":
                return new FetcherDebug(_logger)
                {
                    ID = Guid.Parse(_fetcherOptions.Value.FetcherDebugId),
                    Name  = _fetcherOptions.Value.FetcherDebugName
                };
            case "FETCHER_MOVIE":
                return new FetcherMovie(_fetcherOptions, _logger);
            default:
                throw new NotImplementedException();
        }
    }
}
