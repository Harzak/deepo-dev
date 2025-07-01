using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;

namespace Deepo.DAL.Repository.Interfaces;

public interface IReleaseHistoryRepository
{
    Task<DatabaseOperationResult> AddSpotifyReleaseFetchHistoryAsync(string identifier, DateTime dateUTC, CancellationToken cancellationToken);
    IEnumerable<V_Spotify_Vinyl_Fetch_History> GetSpotifyReleaseFetchHistory(DateTime minDateUTC);
    IEnumerable<V_Spotify_Vinyl_Fetch_History> GetSpotifyReleaseFetchHistory(string identifier);
}