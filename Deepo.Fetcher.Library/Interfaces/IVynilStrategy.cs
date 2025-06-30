namespace Deepo.Fetcher.Library.Interfaces;

public interface IVynilStrategy
{
    Task StartAsync(CancellationToken cancellationToken);
    void OnSuccess(Action action);
    void OnFailure(Action action);
}