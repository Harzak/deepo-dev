using Deepo.Framework.EventArguments;

namespace Deepo.Framework.Interfaces;

public interface IScheduler : IDisposable
{
    event EventHandler<WorkerEventArgs>? ReadyToStart;

    Task StartAsync(CancellationToken cancellationToken);
}
