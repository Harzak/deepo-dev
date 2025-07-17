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

/// <summary>
/// Vinyl fetch pipeline that orchestrates the discovery and processing of vinyl record releases.
/// Discovers Spotify albums from multiple markets and matches them with Discogs releases for storage.
/// </summary>
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

    /// <summary>
    /// Starts the vinyl fetch pipeline operation asynchronously.
    /// Loads history cache and processes albums from French and North American Spotify markets.
    /// </summary>
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

    /// <summary>
    /// Processes Spotify albums from a specific market, attempting to match them with Discogs releases.
    /// Skips albums already processed and tries multiple search strategies for each album.
    /// </summary>
    /// <param name="albums">Asynchronous enumerable of Spotify albums to process.</param>
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

    /// <summary>
    /// Searches for matching Discogs releases by the Spotify album title.
    /// </summary>
    /// <param name="spotifyAlbum">The Spotify album to search for.</param>
    /// <returns>True if at least one matching release was found and inserted; otherwise, false.</returns>
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

    /// <summary>
    /// Searches for matching Discogs releases by the Spotify album's artists.
    /// Iterates through all artists and searches for releases by each artist name.
    /// </summary>
    /// <param name="spotifyAlbum">The Spotify album whose artists to search for.</param>
    /// <returns>True if at least one matching release was found and inserted; otherwise, false.</returns>
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

    /// <summary>
    /// Inserts a Discogs release into the album repository after mapping it to an album model.
    /// </summary>
    /// <param name="release">The Discogs release to insert.</param>
    /// <returns>True if the insertion was successful; otherwise, false.</returns>
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

    /// <summary>
    /// Adds a Spotify album to the fetch history to prevent duplicate processing.
    /// </summary>
    /// <param name="album">The Spotify album to add to history.</param>
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
