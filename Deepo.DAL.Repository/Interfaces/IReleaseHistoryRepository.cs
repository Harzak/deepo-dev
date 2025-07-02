using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface IReleaseHistoryRepository
{
    Task<DatabaseOperationResult> AddSpotifyReleaseFetchHistoryAsync(string identifier, DateTime dateUTC, CancellationToken cancellationToken = default);
    Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByDateAsync(DateTime minDateUTC, CancellationToken cancellationToken = default);
    Task<ReadOnlyCollection<V_Spotify_Vinyl_Fetch_History>> GetSpotifyReleaseFetchHistoryByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
}