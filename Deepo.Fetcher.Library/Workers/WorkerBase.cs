using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Deepo.Fetcher.Library.Workers;

/// <summary>
/// Abstract base class for all worker implementations that extends <see cref="BackgroundService"/>.
/// Provides common functionality for worker lifecycle management, logging, and event handling.
/// </summary>
public abstract class WorkerBase : BackgroundService, IWorker
{
    private readonly ILogger _logger;
    private readonly Stopwatch _timer;

    public event EventHandler<WorkerEventArgs>? WorkerExecuted;
    public string Name { get; set; }  
    public Guid ID { get; set; }
    public Task? Executedtask { get => base.ExecuteTask; }

    protected WorkerBase(ILogger logger) : base()
    {
        _logger = logger;
        _timer = new();
        Name = "DefaultWorker";
    }

    /// <summary>
    /// Starts the worker asynchronously.
    /// </summary>
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
    
    /// <summary>
    /// Executes the worker's main operation
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.ExecuteInternalAsync(stoppingToken).ConfigureAwait(false);
        this.WorkerExecuted?.Invoke(this, new WorkerEventArgs(this));
    }
    
    /// <summary>
    /// Executes the worker's internal logic.
    /// </summary>
    protected abstract Task ExecuteInternalAsync(CancellationToken stoppingToken);

    /// <summary>
    /// Stops the worker asynchronously, performing graceful shutdown or forced termination as needed.
    /// </summary>
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
    
    /// <summary>
    /// Performs forced termination of the worker when graceful shutdown is not possible.
    /// </summary>
    protected abstract void ForcedStop();

    public override void Dispose()
    {
        _timer.Stop();
        base.Dispose();
        GC.SuppressFinalize(this);
    }

    public override string ToString() => $"{GetType()?.Name} | {ID} | {Name}";
}

