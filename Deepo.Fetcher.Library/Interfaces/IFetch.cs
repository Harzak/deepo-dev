namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Internal interface for fetch operations that support counting and success tracking.
/// Provides a contract for disposable fetch operations with status reporting.
/// </summary>
internal interface IFetch : IDisposable
{
    int Count { get; }
    
    public bool Success { get; }

    Task StartAsync(CancellationToken cancellationToken);
}

