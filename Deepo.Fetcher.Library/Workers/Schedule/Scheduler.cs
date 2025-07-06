using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using ITimer = Deepo.Framework.Interfaces.ITimer;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public class Scheduler : TaskScheduler, IScheduler, IDisposable
{
    public event EventHandler<WorkerEventArgs>? ReadyToStart;
    protected IDateTimeFacade TimeProvider { get; }
    protected Dictionary<IWorker, IPlanning> WorkerShedule { get; private set; }

    private readonly ILogger _logger;
    private readonly ITimer _timer;
    private bool disposedValue;

    public Scheduler(IDateTimeFacade timeProvier, ITimer timer, ILogger<Scheduler> logger)
    {
        TimeProvider = timeProvier;
        WorkerShedule = new Dictionary<IWorker, IPlanning>();

        _logger = logger;
        _timer = timer;
        _timer.Elapsed += (sender, e) => EvaluateReadyWorkers();
        _timer.Interval = 1 * 60000;
        _timer.StartTimer();
    }

    public virtual void Start()
    {
    }

    protected override IEnumerable<Task>? GetScheduledTasks()
    {
        throw new NotImplementedException();
    }

    protected override void QueueTask(Task task)
    {
        throw new NotImplementedException();
    }

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        throw new NotImplementedException();
    }

    public DateTime? IsScheduled(IWorker worker)
    {
        WorkerShedule.TryGetValue(worker, out IPlanning? planning);
        return planning?.DateNextStart ?? null;
    }

    public virtual bool RegisterOneShot(IWorker worker)
    {
        IPlanning oneShot = new OneShotPlanning(TimeProvider, _logger);
        UpSertWorker(worker, oneShot);
        EvaluateReadyWorkers();
        return true;
    }

    public virtual bool RegisterHourly(IWorker worker, int startMinute)
    {
        IPlanning hourly = new HourlyPlanning(startMinute, TimeProvider, _logger);
        UpSertWorker(worker, hourly);
        return true;
    }

    public virtual bool RegisterDaily(IWorker worker, int startHour, int startMinute)
    {
        IPlanning daily = new DailyPlanning(startHour, startMinute, TimeProvider, _logger);
        UpSertWorker(worker, daily);
        return true;
    }

    public virtual bool Register(IWorker worker, IPlanning planning)
    {
        UpSertWorker(worker, planning);
        return true;
    }

    public virtual bool Unregister(IWorker worker)
    {
        return Exist(worker) && WorkerShedule.Remove(worker);
    }

    protected virtual void EvaluateReadyWorkers()
    {
        SchedulerLogs.EvaluatingReadyWorkers(_logger, WorkerShedule.Count);

        foreach (KeyValuePair<IWorker, IPlanning> plannedWorker in WorkerShedule.Where(x => x.Key != null))
        {
            EvaluateReadyWorker(plannedWorker);
        }
    }

    protected virtual void EvaluateReadyWorker(KeyValuePair<IWorker, IPlanning> plannedWorker)
    {
        if (plannedWorker.Value != null && plannedWorker.Value.ShouldStart())
        {
            SchedulerLogs.ReadyToStart(_logger, plannedWorker.Key.Name, plannedWorker.Key.ID);
            SchedulerLogs.NextSatrt(_logger, plannedWorker.Key.Name, plannedWorker.Key.ID, plannedWorker.Value.DateNextStart);
            ReadyToStart?.Invoke(this, new WorkerEventArgs(plannedWorker.Key));
            return;
        }
        SchedulerLogs.NotReadyToStart(_logger, plannedWorker.Key.Name, plannedWorker.Key.ID, this.TimeProvider.DateTimeNow());
    }

    protected void UpSertWorker(IWorker worker, IPlanning planning)
    {
        if (Exist(worker))
        {
            WorkerShedule[worker] = planning;
        }
        else
        {
            WorkerShedule.Add(worker, planning);
        }
    }

    private bool Exist(IWorker worker)
    {
        return WorkerShedule != null && WorkerShedule.Any(x => x.Key.ID == worker.ID);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {

            }

            _timer.Elapsed -= (sender, e) => EvaluateReadyWorkers();
            disposedValue=true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}