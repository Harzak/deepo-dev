using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Utils;
using Framework.Common.Utils.Result;
using Framework.Common.Worker;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherVinyl : CancellableWorker
{
    private readonly ILogger _logger;
    private readonly IVynilStrategy _strategy;

    public int FetchSucced { get; private set; }
    public int TotalFetch { get; private set; }

    public FetcherVinyl(IVynilStrategy strategy, ILogger logger)
    : base(logger)
    {
        _strategy = strategy;
        _logger = logger;
    }

    protected override bool CanStop()
    {
        throw new NotImplementedException();
    }

    protected override void ForcedStop()
    {
        throw new NotImplementedException();
    }

    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        _strategy.OnSuccess(() =>
        {
            FetchSucced++;
        });
        await _strategy.StartAsync(stoppingToken).ConfigureAwait(false);

        FetcherLogs.FetchFailed(_logger, TotalFetch - FetchSucced);
        FetcherLogs.FetchSucceed(_logger, FetchSucced);
    }
}

