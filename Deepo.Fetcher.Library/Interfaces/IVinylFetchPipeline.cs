namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Interface for vinyl fetch pipelines that process vinyl record data.
/// Provides tracking of fetch operation results.
/// </summary>
public interface IVinylFetchPipeline
{
    int SuccessfulFetchCount { get; }
    
    int FailedFetchCount { get; }
    
    int IgnoredFetchCount { get; }
    
    int FetchCount => this.SuccessfulFetchCount + this.FailedFetchCount + this.IgnoredFetchCount;

    /// <summary>
    /// Starts the vinyl fetch pipeline operation asynchronously.
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
}