using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Framework.Results;
using System.Collections.ObjectModel;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Factory interface for creating and executing vinyl-related search and discovery strategies.
/// Provides methods to search Discogs and discover Spotify content across different markets.
/// </summary>
public interface IVinylStrategiesFactory
{
    /// <summary>
    /// Searches Discogs for releases by artist name using the appropriate search strategy.
    /// </summary>
    /// <param name="artistName">The name of the artist to search for.</param>
    /// <returns>An operation result containing a list of matching Discogs releases.</returns>
    Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByArtistAsync(string artistName, CancellationToken cancellationToken);
    
    /// <summary>
    /// Searches Discogs for releases by release title using the appropriate search strategy.
    /// </summary>
    /// <param name="releaseTitle">The title of the release to search for.</param>
    /// <returns>An operation result containing a list of matching Discogs releases.</returns>
    Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByTitleAsync(string releaseTitle, CancellationToken cancellationToken);
    
    /// <summary>
    /// Discovers new releases from Spotify's French market using the appropriate discovery strategy.
    /// </summary>
    /// <returns>An asynchronous enumerable of Spotify albums from the French market.</returns>
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyFrenchMarketAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Discovers new releases from Spotify's North American market using the appropriate discovery strategy.
    /// </summary>
    /// <returns>An asynchronous enumerable of Spotify albums from the North American market.</returns>
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyNorthAmericanMarketAsync(CancellationToken cancellationToken);
}