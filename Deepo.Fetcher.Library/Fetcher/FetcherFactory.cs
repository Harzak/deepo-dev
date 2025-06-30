using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Worker.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherFactory : IFetcherFactory
{
    private readonly ILogger _logger;
    private readonly IOptions<FetcherOptions> _fetcherOptions;
    private readonly IVynilStrategy _vinylStrategy;

    public FetcherFactory(IOptions<FetcherOptions> fetcherOptions,
        IVynilStrategy vinylStrategy,
        ILogger<FetcherFactory> logger)
    {
        _logger = logger;
        _fetcherOptions = fetcherOptions;
        _vinylStrategy = vinylStrategy;
    }

    public IWorker CreateFetcher(string code)
    {
        switch (code)
        {
            case "FETCHER_VINYL":
                return new FetcherVinyl(_vinylStrategy, _logger)
                {
                    ID = Guid.Parse(_fetcherOptions.Value.FetcherVinyleId),
                    Name  = _fetcherOptions.Value.FetcherVinyleName
                };
            case "FETCHER_MOVIE":
                return new FetcherMovie(_fetcherOptions, _logger);
            case "FETCHER_DEBUG":
                return new FetcherDebug(_logger)
                {
                    ID = Guid.Parse(_fetcherOptions.Value.FetcherDebugId),
                    Name  = _fetcherOptions.Value.FetcherDebugName
                };
            default:
                throw new NotImplementedException();
        }
    }
}
