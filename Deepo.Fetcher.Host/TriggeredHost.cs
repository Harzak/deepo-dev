using Deepo.DAL.Service.Feature.Fetcher;
using Deepo.Fetcher.Library.Fetcher;
using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Hosting;
using Framework.Common.Worker.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deepo.Fetcher.Library.Configuration.Setting;
using Microsoft.Extensions.Options;
using Framework.Common.Worker.EventArg;

namespace Deepo.Fetcher.Host;

public class TriggeredHost : HostWorker
{
    private readonly IFetcherProvider _fetcherProvider;
    private readonly IFetcherExecutionDBService _fetcherHistory;
    private readonly IHostApplicationLifetime _lifeTime;
    private readonly IOptions<FetcherOptions> _config;

    public TriggeredHost(IFetcherProvider fetcherProvider,
        IFetcherExecutionDBService fetcherHistory,
        IHostApplicationLifetime lifeTime,
        IOptions<FetcherOptions> config,
        ILogger<TriggeredHost> logger)
    : base(Constants.HOST_NAME, Guid.Parse(Constants.HOST_ID), logger)
    {
        _fetcherProvider = fetcherProvider;
        _fetcherHistory = fetcherHistory;
        _lifeTime = lifeTime;
        _config = config;
    }

    protected override bool CanStart()
    {
        return true;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        IWorker? fetcherVinyl = _fetcherProvider.GetFetcherByName(_config.Value.FetcherVinyleName);
        if (fetcherVinyl is not null)
        {
            fetcherVinyl.WorkerExecuted += OnFetcherVinyl_WorkerExecuted;
            base.AddWorker(fetcherVinyl);
            await base.StartAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    private void OnFetcherVinyl_WorkerExecuted(object? sender, WorkerEventArgs e)
    {
        _lifeTime.StopApplication();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await base.StartAllWorkersAsync(stoppingToken).ConfigureAwait(false);
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

