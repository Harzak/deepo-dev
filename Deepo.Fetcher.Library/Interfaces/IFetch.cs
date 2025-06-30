namespace Deepo.Fetcher.Library.Interfaces;

internal interface IFetch : IDisposable
{
    int Count { get; }
    public bool Success { get; }

    Task StartAsync(CancellationToken cancellationToken);

}

