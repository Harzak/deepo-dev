using Deepo.Framework.EventArguments;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for scheduler services that manage worker execution timing and lifecycle events.
/// </summary>
public interface IScheduler : IDisposable
{
    /// <summary>
    /// Occurs when a worker is ready to start execution according to the schedule.
    /// </summary>
    event EventHandler<WorkerEventArgs>? ReadyToStart;

    /// <summary>
    /// Asynchronously starts the scheduler to begin monitoring and triggering worker executions.
    /// </summary>
    Task StartAsync(CancellationToken cancellationToken);
}
