using Deepo.DAL.EF.Models;
using Deepo.Framework.Interfaces;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing fetcher execution lifecycle tracking and logging.
/// </summary>
public interface IFetcherExecutionRepository
{
    /// <summary>
    /// Records the start of a fetcher execution in the database.
    /// </summary>
    Task<bool> LogStartAsync(IWorker worker, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Records the end of a fetcher execution in the database.
    /// </summary>
    Task<bool> LogEndAsync(IWorker worker, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all fetcher execution records.
    /// </summary>
    Task<IEnumerable<V_FetchersLastExecution>> GetFetcherExecutionsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves the most recent fetcher execution record.
    /// </summary>
    Task<V_FetchersLastExecution?> GetLastFetcherExecutionAsync(CancellationToken cancellationToken = default);
}