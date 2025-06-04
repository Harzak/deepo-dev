using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Fetcher.Fetch;
using Deepo.Fetcher.Library.Service.Spotify;
using Framework.Common.Worker.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherFactory : IFetcherFactory
{
    private readonly ILogger _logger;
    private readonly IFetchFactory _fetchFactory;
    private readonly ISpotifyService _spotifyService;
    private readonly IOptions<FetcherOptions> _fetcherOptions;

    public FetcherFactory(IFetchFactory fetchFactory,
        ISpotifyService spotifyService,
        IOptions<FetcherOptions> fetcherOptions,
        ILogger<FetcherFactory> logger)
    {
        _logger = logger;
        _fetchFactory = fetchFactory;
        _fetcherOptions = fetcherOptions;
        _spotifyService = spotifyService;
    }

    public IWorker CreateFetcher(string code)
    {
        switch (code)
        {
            case "FETCHER_VINYL":
                return new FetcherVinyl(_fetchFactory, _spotifyService, _logger)
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
