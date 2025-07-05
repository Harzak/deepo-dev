using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.Result;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Mappers;
using Deepo.Framework.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
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

    public int SuccessfulFetchCount { get; private set; }
    public int FailedFetchCount { get; private set; }
    public int IgnoredFetchCount { get; private set; }
    public int FetchCount => this.SuccessfulFetchCount + this.FailedFetchCount + this.IgnoredFetchCount;


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

    private async Task DiscoverSpotifyMarketAsync(IAsyncEnumerable<DtoSpotifyAlbum> albums, CancellationToken cancellationToken)
    {
        await foreach (DtoSpotifyAlbum spotifyAlbum in albums.ConfigureAwait(false))
        {
            if (_historyIdentifiersCache.Contains(spotifyAlbum.Id!))
            {
                this.IgnoredFetchCount++;
                FetcherLogs.IngoreReleaseInHistory(_logger, spotifyAlbum.Name!, spotifyAlbum.Id!);
                continue;
            }

            await AddInHistory(spotifyAlbum, cancellationToken).ConfigureAwait(false);

            if (await SearchByTitleAsync(spotifyAlbum, cancellationToken).ConfigureAwait(false))
            {
                this.SuccessfulFetchCount++;
                continue;
            }

            if (await SearchByArtistsAsync(spotifyAlbum, cancellationToken).ConfigureAwait(false))
            {
                this.SuccessfulFetchCount++;
                continue;
            }

            this.FailedFetchCount++;
            FetcherLogs.AllStrategiesFailed(_logger, spotifyAlbum.Name!);
        }
    }

    private async Task<bool> SearchByTitleAsync(DtoSpotifyAlbum spotifyAlbum, CancellationToken cancellationToken)
    {
        OperationResultList<DtoDiscogsRelease> searchResult = await _strategiesFactory.SearchDiscogsByTitleAsync(spotifyAlbum.Name!, cancellationToken).ConfigureAwait(false);

        bool success = false;
        if (searchResult.IsSuccess && searchResult.HasContent)
        {
            foreach (DtoDiscogsRelease findRelease in searchResult.Content)
            {
                if (await InsertReleaseAlbumAsync(findRelease, cancellationToken).ConfigureAwait(false))
                {
                    FetcherLogs.SuccessStrategy(_logger, strategyName: "discogs search by title", spotifyAlbum.Name!);
                    success = true;
                }
            }
        }
        return success;
    }

    private async Task<bool> SearchByArtistsAsync(DtoSpotifyAlbum spotifyAlbum, CancellationToken cancellationToken)
    {
        bool success = false;
        foreach (DtoSpotifyArtist artist in spotifyAlbum.Artists!)
        {
            OperationResultList<DtoDiscogsRelease> searchResult = await _strategiesFactory.SearchDiscogsByArtistAsync(artist.Name!, cancellationToken).ConfigureAwait(false);

            if (searchResult.IsSuccess && searchResult.HasContent)
            {
                foreach (DtoDiscogsRelease findRelease in searchResult.Content)
                {
                    if (await InsertReleaseAlbumAsync(findRelease, cancellationToken).ConfigureAwait(false))
                    {
                        FetcherLogs.SuccessStrategy(_logger, strategyName: "discogs search by artists", artist.Name!);
                        success = true;
                    }
                }
            }
        }
        return success;
    }

    private async Task<bool> InsertReleaseAlbumAsync(DtoDiscogsRelease release, CancellationToken cancellationToken)
    {
        AlbumModel mappedModel = release.MapToAlbum();
        DatabaseOperationResult insertResult = await _releaseAlbumRepository.InsertAsync(mappedModel, cancellationToken).ConfigureAwait(false);
        if (insertResult.IsSuccess)
        {
            FetcherLogs.InsertSucceed(_logger, release.Title!);
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
        this.SuccessfulFetchCount = 0;
        this.FailedFetchCount = 0;
        this.IgnoredFetchCount = 0;
        _historyIdentifiersCache = [];
    }
}
