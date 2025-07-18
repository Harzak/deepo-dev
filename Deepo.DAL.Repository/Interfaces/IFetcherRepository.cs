using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

/// <summary>
/// Repository interface for managing fetcher configuration data and metadata operations.
/// </summary>
public interface IFetcherRepository
{
    /// <summary>
    /// Retrieves all fetcher configurations.
    /// </summary>
    Task<ReadOnlyCollection<Models.Fetcher>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves all fetchers with extended information.
    /// </summary>
    Task<ReadOnlyCollection<Models.V_FetcherExtended>> GetAllExtendedAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a fetcher by its name.
    /// </summary>
    Task<Models.Fetcher?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a fetcher by its unique identifier.
    /// </summary>
    Task<Models.Fetcher?> GetByIdAsync(string identifier, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves extended fetcher information by identifier.
    /// </summary>
    Task<Models.V_FetcherExtended?> GetExtendedAsync(string id, CancellationToken cancellationToken = default);
}

