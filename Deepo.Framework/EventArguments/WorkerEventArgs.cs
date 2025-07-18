using Deepo.Framework.Interfaces;

namespace Deepo.Framework.EventArguments;

public class WorkerEventArgs : EventArgs
{
    public IWorker Worker { get; set; }

    public WorkerEventArgs(IWorker worker)
    {
        this.Worker = worker;
    }
}