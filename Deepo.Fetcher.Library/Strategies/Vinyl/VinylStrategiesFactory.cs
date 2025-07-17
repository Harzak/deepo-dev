using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

/// <summary>
/// Creates and manages vinyl-related search strategies.
/// Coordinates various strategies for searching Discogs and Spotify API contents across different markets.
/// </summary>
public class VinylStrategiesFactory : IVinylStrategiesFactory
{
    private readonly ILogger _logger;

    private readonly DiscogsSearchByArtistsStrategy _strategyDiscogsByArtists;
    private readonly DiscogsSearchByReleaseTitleStrategy _strategyDiscogsByTitle;
    private readonly SpotifyDiscoverByMarketStrategy _strategySpotifyByMarket;

    public VinylStrategiesFactory(IDiscogRepository discogRepository,  
        ISpotifyRepository spotifyRepository, 
        ILogger<VinylStrategiesFactory> logger)
    {
        _logger = logger;

        _strategyDiscogsByArtists = new DiscogsSearchByArtistsStrategy(discogRepository, logger);
        _strategyDiscogsByTitle =  new DiscogsSearchByReleaseTitleStrategy(discogRepository, logger);
        _strategySpotifyByMarket = new SpotifyDiscoverByMarketStrategy(spotifyRepository, logger);
    }

    /// <summary>
    /// Discovers new releases from Spotify's French market.
    /// </summary>
    /// <returns>An asynchronous enumerable of Spotify albums from the French market.</returns>
    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyFrenchMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in _strategySpotifyByMarket.DiscoverFrenchMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    /// <summary>
    /// Discovers new releases from Spotify's North American market.
    /// </summary>
    /// <returns>An asynchronous enumerable of Spotify albums from the North American market.</returns>
    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyNorthAmericanMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in _strategySpotifyByMarket.DiscoverNorthAmericanMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    /// <summary>
    /// Searches Discogs for releases by artist name.
    /// </summary>
    /// <param name="artistName">The name of the artist to search for.</param>
    /// <returns>An operation result containing a list of matching Discogs releases.</returns>
    public async Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByArtistAsync(string artistName, CancellationToken cancellationToken)
    {
        return await _strategyDiscogsByArtists.SearchAsync(artistName, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches Discogs for releases by release title.
    /// </summary>
    /// <param name="releaseTitle">The title of the release to search for.</param>
    /// <returns>An operation result containing a list of matching Discogs releases.</returns>
    public async Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByTitleAsync(string releaseTitle, CancellationToken cancellationToken)
    {
        return await _strategyDiscogsByTitle.SearchAsync(releaseTitle, cancellationToken).ConfigureAwait(false);
    }
}
