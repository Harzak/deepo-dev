using Deepo.Framework.EventArguments;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for worker services that perform background tasks with lifecycle management and event notifications.
/// </summary>
public interface IWorker
{
    /// <summary>
    /// Gets or sets the name identifier for the worker.
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the unique identifier for the worker instance.
    /// </summary>
    Guid ID { get; set; }
    
    /// <summary>
    /// Gets the task representing the worker's execution, if currently running.
    /// </summary>
    Task? ExecuteTask { get; }

    /// <summary>
    /// Occurs when the worker has completed its execution cycle.
    /// </summary>
    event EventHandler<WorkerEventArgs> WorkerExecuted;

    /// <summary>
    /// Asynchronously starts the worker's execution.
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Asynchronously stops the worker's execution.
    /// </summary>
    Task StopAsync(CancellationToken cancellationToken);
}