namespace Deepo.Fetcher.Library.Interfaces;

public interface IVinylFetchPipeline
{
    Task StartAsync(CancellationToken cancellationToken);
    void OnStrategySuccess(Action action);
    void OnStrategyFailure(Action action);
}