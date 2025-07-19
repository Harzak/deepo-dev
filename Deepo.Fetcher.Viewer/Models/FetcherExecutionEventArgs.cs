using System;

namespace Deepo.Fetcher.Viewer.Models;

/// <summary>
/// Provides event data for fetcher execution events, containing information about the fetcher and its execution time.
/// </summary>
public sealed class FetcherExecutionEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the unique identifier of the fetcher that was executed.
    /// </summary>
    public string FetcherIdentifier { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the fetcher execution started.
    /// </summary>
    public DateTime StartedAt { get; set; }

    public FetcherExecutionEventArgs(string fetcherIdentifier, DateTime startedAt)
    {
        FetcherIdentifier = fetcherIdentifier;
        StartedAt = startedAt;
    }
}
