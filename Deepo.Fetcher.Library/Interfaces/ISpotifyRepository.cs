using Deepo.Fetcher.Library.Utils;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Repository interface for accessing Spotify API data.
/// Provides methods to retrieve music releases and albums from Spotify.
/// </summary>
public interface ISpotifyRepository
{
    /// <summary>
    /// Retrieves new releases from Spotify for a specific market via search functionality.
    /// Returns an asynchronous enumerable to handle large result sets efficiently.
    /// </summary>
    /// <param name="market">The market (country) to search for new releases.</param>
    /// <returns>An asynchronous enumerable of HTTP service results containing Spotify albums.</returns>
    IAsyncEnumerable<HttpServiceResult<Dto.Spotify.DtoSpotifyAlbum>> GetNewReleasesViaSearch(string market, CancellationToken cancellationToken);
}
