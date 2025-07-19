using Deepo.Framework.Interfaces;

namespace Deepo.Framework.EventArguments;

/// <summary>
/// Provides event data for worker-related events, containing information about the worker that triggered the event.
/// </summary>
public class WorkerEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the worker instance that triggered the event.
    /// </summary>
    public IWorker Worker { get; set; }

    public WorkerEventArgs(IWorker worker)
    {
        this.Worker = worker;
    }
}