using Deepo.Fetcher.Host.LogMessages;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Host.Hosting;

public abstract class HostWorker :  HostWorkerBase
{
    private bool _disposedValue;
    private readonly Stopwatch _timer;
    private readonly List<IWorker> _workers;

    protected HostWorker(string name, Guid id, ILogger logger) : base(logger)
    {
        _timer = new();
        _workers = [];
        Name = name;
        Id = id;
    }

    #region Start
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (CanStart())
        {
            _timer.Start();
            HostLogs.HostStarted(Logger, GetType(), DateTime.Now);
        }
        return base.StartAsync(cancellationToken);
    }

    protected abstract bool CanStart();
    #endregion

    #region Stop
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (CanStop())
        {
            Dispose();
            HostLogs.HostStarted(Logger, GetType(), DateTime.Now);
        }
        else
        {
            ForcedStop();
            HostLogs.HostForcedStopped(Logger, GetType(), DateTime.Now);
        }
        _timer.Stop();
        HostLogs.HostAchieved(Logger, GetType(), _timer.Elapsed);

        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    protected virtual bool CanStop()
    {
        if (base.ExecuteTask != null && !base.ExecuteTask.IsCompleted)
        {
            HostLogs.HostCannotStop(Logger, GetType());
            return false;
        }
        return true;
    }

    protected virtual void ForcedStop()
    {

    }
    #endregion

    #region Worker management
    protected override async Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
    {
        if (worker != null && _workers.Any(x => x.ID == worker.ID))
        {
            await worker.StartAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException("Attempt to run an unreferenced worker");
        }
    }

    protected override async Task StartAllWorkersAsync(CancellationToken cancellationToken)
    {
        foreach (IWorker worker in _workers.Where(x => x != null))
        {
            await StartWorkerAsync(worker, cancellationToken).ConfigureAwait(false);
        }
    }

    protected override async Task StartManyWorkersAsync(IEnumerable<IWorker> workers, CancellationToken cancellationToken)
    {
        if (workers != null)
        {
            List<Task> jobsTasks = new();
            foreach (IWorker worker in workers.Where(x => x != null))
            {
                Task task = Task.Run(() => StartWorkerAsync(worker, cancellationToken), cancellationToken);
                jobsTasks.Add(task);
            }
            await Task.WhenAll(jobsTasks).ConfigureAwait(false);
        }
    }

    protected override async Task StopWorkersAsync(IWorker worker, CancellationToken cancellationToken)
    {
        if (worker != null && _workers.Any(x => x.ID == worker.ID))
        {
            await worker.StopAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException("Attempt to stop an unreferenced worker");
        }
    }

    protected override async Task StopAllWorkersAsync(CancellationToken cancellationToken)
    {
        foreach (IWorker worker in _workers.Where(x => x != null))
        {
            await StopWorkersAsync(worker, cancellationToken).ConfigureAwait(false);
        }
    }

    protected virtual void AddWorker(IWorker worker)
    {
        if (worker is null || worker.ID == Guid.Empty || string.IsNullOrEmpty(worker.Name) || _workers.Any(x => x.ID == worker.ID || x.Name == worker.Name))
        {
            throw new InvalidOperationException("Attemp to add a null or already existing worker");
        }
        else
        {
            _workers.Add(worker);
        }
    }

    protected virtual void AddWorkers(ReadOnlyCollection<IWorker> workers)
    {
        if (workers != null && workers.Count > 0)
        {
            foreach (IWorker worker in workers)
            {
                AddWorker(worker);
            }
        }
    }
    #endregion

    public override string ToString() => $"{GetType()?.Name} | {Id} | {Name}";

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            _timer.Stop();
            _disposedValue=true;
        }
    }

    public override void Dispose()
    {
        Dispose(disposing: true);
        base.Dispose();
        GC.SuppressFinalize(this);
    }
}
