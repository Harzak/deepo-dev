using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.Result;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Mappers;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Globalization;

namespace Deepo.Fetcher.Library.Fetcher.Vinyl;

public sealed class VinylFetchPipeline : IVinylFetchPipeline, IDisposable
{
    private readonly ILogger<VinylFetchPipeline> _logger;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IVinylStrategiesFactory _strategiesFactory;
    private readonly IReleaseHistoryRepository _historyRepository;

    private IEnumerable<string> _historyIdentifiersCache;

    private Action? _onStrategySuccess;
    private Action? _onStrategyFailure;

    public VinylFetchPipeline( 
        IVinylStrategiesFactory strategiesFactory,
        IReleaseAlbumRepository releaseAlbumRepository,
        IReleaseHistoryRepository historyRepository,
        ILogger<VinylFetchPipeline> logger)
    {
        _strategiesFactory = strategiesFactory;
        _releaseAlbumRepository = releaseAlbumRepository;
        _historyRepository = historyRepository;
        _logger = logger;

        _historyIdentifiersCache = [];
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _historyIdentifiersCache = (await _historyRepository.GetSpotifyReleaseFetchHistoryByDateAsync(DateTime.UtcNow.AddDays(-7), cancellationToken).ConfigureAwait(false))
                                    .Select(x => x.Identifier)
                                    .Distinct();

        IAsyncEnumerable<DtoSpotifyAlbum> frenchReleases = _strategiesFactory.DiscoverSpotifyFrenchMarketAsync(cancellationToken);
        await DiscoverSpotifyMarketAsync(frenchReleases, cancellationToken).ConfigureAwait(false);

        IAsyncEnumerable<DtoSpotifyAlbum> usReleases = _strategiesFactory.DiscoverSpotifyNorthAmericanMarketAsync(cancellationToken);
        await DiscoverSpotifyMarketAsync(usReleases, cancellationToken).ConfigureAwait(false);
    }

    public void OnStrategySuccess(Action action)
    {
        _onStrategySuccess = action;
    }

    public void OnStrategyFailure(Action action)
    {
        _onStrategyFailure = action;
    }

    private async Task DiscoverSpotifyMarketAsync(IAsyncEnumerable<DtoSpotifyAlbum> albums, CancellationToken cancellationToken)
    {
        await foreach (DtoSpotifyAlbum spotifyAlbum in albums.ConfigureAwait(false))
        {
            if (_historyIdentifiersCache.Contains(spotifyAlbum.Id!))
            {
                FetcherLogs.IngoreReleaseInHistory(_logger, spotifyAlbum.Name!, spotifyAlbum.Id!);
                continue;
            }

            await AddInHistory(spotifyAlbum, cancellationToken).ConfigureAwait(false);

            if (await SearchByTitleAsync(spotifyAlbum, cancellationToken).ConfigureAwait(false))
            {
                _onStrategySuccess?.Invoke();
                continue;
            }

            if (await SearchByArtistsAsync(spotifyAlbum, cancellationToken).ConfigureAwait(false))
            {
                _onStrategySuccess?.Invoke();
                continue;
            }

            _onStrategyFailure?.Invoke();
            FetcherLogs.AllStrategiesFailed(_logger, spotifyAlbum.Name!);
        }
    }

    private async Task<bool> SearchByTitleAsync(DtoSpotifyAlbum spotifyAlbum, CancellationToken cancellationToken)
    {
        OperationResult<DtoDiscogsRelease> searchResult = await _strategiesFactory.SearchDiscogsByTitleAsync(spotifyAlbum.Name!, cancellationToken).ConfigureAwait(false);

        if (searchResult.IsSuccess)
        {
            if (await InsertReleaseAlbumAsync(searchResult.Content, cancellationToken).ConfigureAwait(false))
            {
                FetcherLogs.SuccessStrategy(_logger, $"{nameof(_strategiesFactory.SearchDiscogsByTitleAsync)} by title", spotifyAlbum.Name!);
                return true;
            }
        }
        return false;
    }

    private async Task<bool> SearchByArtistsAsync(DtoSpotifyAlbum spotifyAlbum, CancellationToken cancellationToken)
    {
        foreach (DtoSpotifyArtist artist in spotifyAlbum.Artists!)
        {
            OperationResult<DtoDiscogsRelease> searchResult = await _strategiesFactory.SearchDiscogsByArtistAsync(artist.Name!, cancellationToken).ConfigureAwait(false);

            if (searchResult.IsSuccess)
            {
                if( await InsertReleaseAlbumAsync(searchResult.Content, cancellationToken).ConfigureAwait(false))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private async Task<bool> InsertReleaseAlbumAsync(DtoDiscogsRelease release, CancellationToken cancellationToken)
    {
        AlbumModel mappedModel = release.MapToAlbum();
        DatabaseOperationResult insertResult = await _releaseAlbumRepository.InsertAsync(mappedModel, cancellationToken).ConfigureAwait(false);
        if (insertResult.IsSuccess)
        {
            FetcherLogs.SuccessStrategy(_logger, $"{nameof(_strategiesFactory.SearchDiscogsByArtistAsync)}", release.Title!);
            return true;
        }
        else
        {
            FetcherLogs.InsertFailed(_logger, release.Title!, insertResult.ErrorMessage);
            return false;
        }
    }

    private async Task AddInHistory(DtoSpotifyAlbum album, CancellationToken cancellationToken)
    {
        if (album.Id != null)
        {
            await _historyRepository.AddSpotifyReleaseFetchHistoryAsync(
                album.Id.ToString(CultureInfo.InvariantCulture),
                DateTime.UtcNow,
                cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _onStrategyFailure = null;
        _onStrategySuccess = null;
        _historyIdentifiersCache = [];
    }
}
