using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Framework.Results;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Repository interface for accessing Discogs API data.
/// Provides methods to search for albums and releases from the Discogs database.
/// </summary>
public interface IDiscogRepository
{
    /// <summary>
    /// Searches for albums by release title and year from the Discogs database.
    /// </summary>
    /// <param name="releaseTitle">The title of the release to search for.</param>
    /// <param name="year">The year of the release to search for.</param>
    /// <param name="cancellationToken">Cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An operation result containing a collection of matching Discogs albums.</returns>
    Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken);
    
    /// <summary>
    /// Searches for albums by artist name from the Discogs database.
    /// </summary>
    /// <param name="nameArtist">The name of the artist to search for.</param>
    /// <param name="cancellationToken">Cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An operation result containing a collection of matching Discogs albums.</returns>
    Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves a specific release by its ID from the Discogs database.
    /// </summary>
    /// <param name="id">The unique identifier of the release.</param>
    /// <param name="cancellationToken">Cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An operation result containing the Discogs release details.</returns>
    Task<OperationResult<DtoDiscogsRelease>> GetReleaseByID(string id, CancellationToken cancellationToken);
}
