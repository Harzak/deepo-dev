namespace Deepo.Fetcher.Library.Interfaces;

public interface IVinylFetchPipeline
{
    int SuccessfulFetchCount { get; }
    int FailedFetchCount { get; }
    int IgnoredFetchCount { get; }
    int FetchCount => this.SuccessfulFetchCount + this.FailedFetchCount + this.IgnoredFetchCount;

    Task StartAsync(CancellationToken cancellationToken);
}