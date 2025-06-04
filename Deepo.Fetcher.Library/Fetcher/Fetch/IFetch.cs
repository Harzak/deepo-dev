namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal interface IFetch : IDisposable
{
    int Count { get; }
    public bool Success { get; }

    Task StartAsync(CancellationToken cancellationToken);

}

