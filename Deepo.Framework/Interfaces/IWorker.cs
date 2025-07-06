using Deepo.Framework.EventArguments;

namespace Deepo.Framework.Interfaces;

public interface IWorker
{
    string Name { get; set; }
    Guid ID { get; set; }
    Task? ExecuteTask { get; }

    event EventHandler<WorkerEventArgs> WorkerExecuted;

    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
}