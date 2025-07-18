using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for tracking and managing release fetch history for external data providers.
/// </summary>
public interface IReleaseHistoryRepository
{
    /// <summary>
    /// Records a Spotify release fetch operation in the history log.
    /// </summary>
    Task<DatabaseOperationResult> AddSpotifyReleaseFetchHistoryAsync(string identifier, DateTime dateUTC, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves Spotify release fetch history records from a specified minimum date.
    /// </summary>
    Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByDateAsync(DateTime minDateUTC, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves Spotify release fetch history records for a specific identifier.
    /// </summary>
    Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
}