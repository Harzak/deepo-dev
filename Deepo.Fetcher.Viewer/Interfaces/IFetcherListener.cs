using Deepo.Fetcher.Viewer.Models;
using System;

namespace Deepo.Fetcher.Viewer.Interfaces;

/// <summary>
/// Defines the contract for listening to database changes related to fetcher operations.
/// </summary>
public interface IFetcherListener : IDisposable
{
    /// <summary>
    /// Occurs when a new HTTP request log entry is added to the database.
    /// </summary>
    event EventHandler<HttpRequestLogEventArgs>? HttpRequestLogRowAdded;
    
    /// <summary>
    /// Occurs when a new vinyl release is added to the database.
    /// </summary>
    event EventHandler<GridModelEventArgs>? VinylReleaseRowAdded;
    
    /// <summary>
    /// Occurs when a new fetcher execution is recorded in the database.
    /// </summary>
    event EventHandler<FetcherExecutionEventArgs>? FetcherExecutionRowAdded;

    /// <summary>
    /// Starts monitoring database changes for fetcher-related operations.
    /// </summary>
    void StartListener();
}
