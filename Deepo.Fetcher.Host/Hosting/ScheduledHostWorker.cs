using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Host.Hosting;

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

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _scheduler.StartAsync(stoppingToken).ConfigureAwait(false);
    }

    private async void OnWorkerReadyToStartAsync(object? sender, WorkerEventArgs? e)
    {
        if (e?.Worker != null)
        {
            await StartWorkerAsync(e.Worker, CancellationToken.None).ConfigureAwait(false);
        }
    }

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