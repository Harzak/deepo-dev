using Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing HTTP request data related to fetcher operations.
/// </summary>
public interface IFetcherHttpRequestRepository
{
    /// <summary>
    /// Retrieves the most recent HTTP request record.
    /// </summary>
    Task<HttpRequest?> GetLastAsync(CancellationToken cancellationToken = default);
}
