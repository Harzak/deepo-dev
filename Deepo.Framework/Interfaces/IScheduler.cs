using Deepo.Framework.EventArguments;

namespace Deepo.Framework.Interfaces;

public interface IScheduler : IDisposable
{
    event EventHandler<WorkerEventArgs>? ReadyToStart;

    void Start();

    DateTime? IsScheduled(IWorker worker);

    bool Register(IWorker worker, IPlanning planning);

    bool RegisterOneShot(IWorker worker);

    bool RegisterHourly(IWorker worker, int startMinute);

    bool RegisterDaily(IWorker worker, int startHour, int startMinute);

    bool Unregister(IWorker worker);
}

public delegate void SchedulerChanged(IWorker sender);
