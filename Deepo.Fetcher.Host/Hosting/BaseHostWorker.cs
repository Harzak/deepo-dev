using Deepo.Fetcher.Viewer.Interfaces;
using Deepo.Fetcher.Viewer.LogMessages;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Viewer.Hosting;

/// <summary>
/// Provides a base implementation for host workers that manage the execution lifecycle of fetcher tasks.
/// </summary>
public abstract class BaseHostWorker : BackgroundService, IHostWorker
{
    private readonly Stopwatch _timer;
    private readonly ILogger _logger;

    /// <summary>
    /// Gets or sets the human-readable name of this host worker.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for this host worker instance.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets the fetcher provider used to retrieve and manage fetcher worker instances.
    /// </summary>
    protected IFetcherProvider FetcherProvider { get; }

    protected BaseHostWorker(string name, Guid id, IFetcherProvider fetcherProvider, ILogger logger)
    {
        _timer = new();
        _logger = logger;
        this.FetcherProvider = fetcherProvider;
        this.Name = name;
        this.Id = id;
    }

    /// <summary>
    /// Starts the host worker asynchronously, performing initialization.
    /// </summary>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (CanStart())
        {
            _timer.Start();
            HostLogs.HostStarted(_logger, GetType(), DateTime.Now);
        }
        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Determines whether the host worker can start based on implementation-specific criteria.
    /// </summary>
    protected abstract bool CanStart();

    /// <summary>
    /// Stops the host worker asynchronously, handling graceful shutdown or forced termination as needed.
    /// </summary>
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

    /// <summary>
    /// Determines whether the host worker can stop gracefully based on the current execution state.
    /// </summary>
    protected virtual bool CanStop()
    {
        if (base.ExecuteTask != null && !base.ExecuteTask.IsCompleted)
        {
            HostLogs.HostCannotStop(_logger, GetType());
            return false;
        }
        return true;
    }

    /// <summary>
    /// Performs cleanup operations when a graceful stop is not possible.
    /// </summary>
    protected virtual void ForcedStop()
    {

    }

    /// <summary>
    /// Starts a fetcher worker asynchronously with proper validation and error handling.
    /// </summary>
    /// <param name="worker">The worker instance to start. Cannot be null.</param>
    protected virtual async Task StartWorkerAsync(IWorker worker, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(worker, nameof(worker));
        await worker.StartAsync(cancellationToken).ConfigureAwait(false);
    }

    public override string ToString() => $"{GetType()?.Name} | {Id} | {Name}";
}
