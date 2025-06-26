using Deepo.Fetcher.Library.Fetcher.Fetch;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Service;
using Deepo.Fetcher.Library.Service.Spotify;
using Framework.Common.Worker;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherVinyl : CancellableWorker
{
    private readonly ILogger _logger;
    private readonly IFetchFactory _fetchFactory;
    private readonly ISpotifyService _spotifyService;

    public int FetchSucced { get; private set; }
    public int TotalFetch { get; private set; }

    public FetcherVinyl(IFetchFactory fetchFactory, ISpotifyService spotifyService, ILogger logger)
    : base(logger)
    {
        _logger = logger;
        _fetchFactory = fetchFactory;
        _spotifyService = spotifyService;
    }

    protected override bool CanStop()
    {
        throw new NotImplementedException();
    }

    protected override void ForcedStop()
    {
        throw new NotImplementedException();
    }

    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        await foreach (HttpServiceResult<Dto.Spotify.Album> result in _spotifyService.GetNewReleasesViaSearch(stoppingToken).ConfigureAwait(false))
        {
            using (IFetch fetch = _fetchFactory.CreateFetchVinyl(result.Content))
            {
                await fetch.StartAsync(stoppingToken).ConfigureAwait(false);

                if (fetch.Success)
                {
                    FetchSucced++;
                }

                TotalFetch++;
            }
        }
        FetcherLogs.FetchFailed(_logger, TotalFetch - FetchSucced);
        FetcherLogs.FetchSucceed(_logger, FetchSucced);
    }
}

