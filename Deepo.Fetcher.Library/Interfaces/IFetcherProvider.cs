using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Provides access to fetcher instances based on various criteria such as name or identifier.
/// </summary>
public interface IFetcherProvider
{
    /// <summary>
    /// Retrieves all available fetcher workers.
    /// </summary>
    /// <returns>A collection of all available fetcher workers.</returns>
    Task<IEnumerable<IWorker>> GetAllFetcherAsync();
    
    /// <summary>
    /// Retrieves a specific fetcher worker by its name.
    /// </summary>
    /// <param name="name">The name of the fetcher to retrieve.</param>
    /// <returns>The fetcher worker if found; otherwise, null.</returns>
    Task<IWorker?> GetFetcherByNameAsync(string name);
    
    /// <summary>
    /// Retrieves a specific fetcher worker by its identifier.
    /// </summary>
    /// <param name="identifier">The unique identifier of the fetcher to retrieve.</param>
    /// <returns>The fetcher worker if found; otherwise, null.</returns>
    Task<IWorker?> GetFetcherByIdAsync(string identifier);
}