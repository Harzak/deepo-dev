using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Workers;

public abstract class CancellableWorker : WorkerBase, ICancellableTask
{
    private readonly ILogger _logger;
    protected CancellableWorker(ILogger logger) : base(logger)
    {
        _logger = logger;
    }

    public async Task Cancel(CancellationToken cancellationToken, string reason = "")
    {
        WorkerLogs.WorkerHasBeenCanceled(_logger, Name, ID, string.IsNullOrEmpty(reason) ? ("Reason: " + reason) : string.Empty);
        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }
}