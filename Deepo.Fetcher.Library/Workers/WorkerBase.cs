using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Deepo.Fetcher.Library.Workers;

public abstract class WorkerBase : BackgroundService, IWorker
{
    private readonly ILogger _logger;
    private readonly Stopwatch _timer;

    public string Name { get; set; }
    public Guid ID { get; set; }
    public Task? Executedtask { get => base.ExecuteTask; }

    public event EventHandler<WorkerEventArgs>? WorkerExecuted;

    protected WorkerBase(ILogger logger) : base()
    {
        _logger = logger;
        _timer = new();
        Name = "DefaultWorker";
    }

    #region Start / Execution
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        WorkerLogs.WorkerTryStart(_logger, Name, ID, DateTime.Now);
        if (CanStart())
        {
            _timer.Start();
            await base.StartAsync(cancellationToken).ConfigureAwait(false);
            WorkerLogs.WorkerStarted(_logger, Name, ID, DateTime.Now);
        }
        else
        {
            WorkerLogs.WorkerUnhautorizedToStart(_logger, Name, ID);
        }
    }
    protected virtual bool CanStart()
    {
        return true;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.ExecuteInternalAsync(stoppingToken).ConfigureAwait(false);
        this.WorkerExecuted?.Invoke(this, new WorkerEventArgs(this));
    }
    protected abstract Task ExecuteInternalAsync(CancellationToken stoppingToken);
    #endregion

    #region Stop / Dispose
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        WorkerLogs.WorkerTryStop(_logger, Name, ID, DateTime.Now);
        if (!CanStop())
        {
            WorkerLogs.WorkerCannotStop(_logger, Name, ID);
            ForcedStop();
            WorkerLogs.WorkerForcedStopped(_logger, Name, ID, DateTime.Now);
        }
        _timer.Stop();

        Task stopTask = base.StopAsync(cancellationToken);
        WorkerLogs.WorkerStopped(_logger, Name, ID, DateTime.Now);
        this.WorkerExecuted?.Invoke(this, new WorkerEventArgs(this));
        return stopTask;
    }
    protected abstract bool CanStop();
    protected abstract void ForcedStop();

    public override void Dispose()
    {
        _timer.Stop();
        base.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion

    public override string ToString() => $"{GetType()?.Name} | {ID} | {Name}";
}

