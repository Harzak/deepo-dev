using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Library.Fetcher;
using Framework.Common.Worker.Hosting;
using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule;

namespace Deepo.Fetcher.Host;

internal sealed class ContinuousHost : HostPlannedWorker
{
    private readonly IFetcherProvider _fetcherProvider;
    private readonly IFetcherExecutionDBService _fetcherHistory;

    public ContinuousHost(IFetcherProvider fetcherProvider, 
        IFetcherExecutionDBService fetcherHistory,
        IScheduler scheduler, 
        ILogger<ContinuousHost> logger)
    : base(Constants.HOST_NAME, Guid.Parse(Constants.HOST_ID), scheduler, logger)
    {
        _fetcherProvider = fetcherProvider;
        _fetcherHistory = fetcherHistory;
    }

    protected override bool CanStart()
    {
        return true;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        base.AddWorkers(_fetcherProvider.GetAllFetcher());
        await base.StartAsync(cancellationToken).ConfigureAwait(false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        base.Scheduler.Start();
        return Task.CompletedTask;
    }

    protected override async Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
    {
        _fetcherHistory.LogStart(worker);
        await base.StartWorkerAsync(worker, cancellationToken).ConfigureAwait(false);
        _fetcherHistory.LogEnd(worker);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }
}

