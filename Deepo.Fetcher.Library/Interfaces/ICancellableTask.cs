namespace Deepo.Fetcher.Library.Interfaces;

public interface ICancellableTask
{
    Task Cancel(CancellationToken cancellationToken, string reason = "");
}