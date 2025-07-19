using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Viewer.Hosting;

/// <summary>
/// Implements a scheduled host worker that manages fetcher task execution based on scheduling events.
/// </summary>
public sealed class ScheduledHostWorker : BaseHostWorker
{
    private readonly IScheduler _scheduler;
    private readonly IFetcherExecutionRepository _fetcherHistory;

    public ScheduledHostWorker(IScheduler scheduler, IFetcherExecutionRepository fetcherHistory, IFetcherProvider fetcherProvider, ILogger<BaseHostWorker> logger)
        : base(Constants.HOST_NAME, Guid.Parse(Constants.HOST_ID), fetcherProvider, logger)
    {
        _scheduler = scheduler;
        _fetcherHistory = fetcherHistory;
        _scheduler.ReadyToStart += OnWorkerReadyToStartAsync;
    }

    /// <summary>
    /// Executes the scheduled host worker by starting the scheduler and entering the event-driven execution loop.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _scheduler.StartAsync(stoppingToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Handles scheduler notifications when a fetcher worker is ready to start execution.
    /// This event handler is called automatically when the scheduler determines a fetcher's scheduled time has arrived.
    /// </summary>
    private async void OnWorkerReadyToStartAsync(object? sender, WorkerEventArgs? e)
    {
        if (e?.Worker != null)
        {
            await StartWorkerAsync(e.Worker, CancellationToken.None).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Starts a fetcher worker.
    /// </summary>
    protected override async Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
    {
        await _fetcherHistory.LogStartAsync(worker, cancellationToken).ConfigureAwait(false);
        await base.StartWorkerAsync(worker, cancellationToken).ConfigureAwait(false);
    }

    protected override bool CanStart() => true;

    public override void Dispose()
    {
        _scheduler.ReadyToStart -= OnWorkerReadyToStartAsync;
        base.Dispose();
    }
}