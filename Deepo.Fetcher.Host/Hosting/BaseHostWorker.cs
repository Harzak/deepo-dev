using Deepo.Fetcher.Host.Interfaces;
using Deepo.Fetcher.Host.LogMessages;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Host.Hosting;

public abstract class BaseHostWorker : BackgroundService, IHostWorker
{
    private readonly Stopwatch _timer;
    private readonly ILogger _logger;

    public string Name { get; set; }
    public Guid Id { get; set; }
    protected IFetcherProvider FetcherProvider { get; }

    protected BaseHostWorker(string name, Guid id, IFetcherProvider fetcherProvider, ILogger logger)
    {
        _timer = new();
        _logger = logger;
        this.FetcherProvider = fetcherProvider;
        this.Name = name;
        this.Id = id;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (CanStart())
        {
            _timer.Start();
            HostLogs.HostStarted(_logger, GetType(), DateTime.Now);
        }
        return base.StartAsync(cancellationToken);
    }

    protected abstract bool CanStart();

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (CanStop())
        {
            HostLogs.HostStarted(_logger, GetType(), DateTime.Now);
        }
        else
        {
            ForcedStop();
            HostLogs.HostForcedStopped(_logger, GetType(), DateTime.Now);
        }
        _timer.Stop();
        HostLogs.HostAchieved(_logger, GetType(), _timer.Elapsed);

        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    protected virtual bool CanStop()
    {
        if (base.ExecuteTask != null && !base.ExecuteTask.IsCompleted)
        {
            HostLogs.HostCannotStop(_logger, GetType());
            return false;
        }
        return true;
    }

    protected virtual void ForcedStop()
    {

    }

    protected virtual async Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(worker, nameof(worker));
        await worker.StartAsync(cancellationToken).ConfigureAwait(false);
    }

    public override string ToString() => $"{GetType()?.Name} | {Id} | {Name}";
}
