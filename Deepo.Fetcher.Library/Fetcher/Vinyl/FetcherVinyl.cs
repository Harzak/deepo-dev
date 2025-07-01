using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Utils;
using Framework.Common.Utils.Result;
using Framework.Common.Worker;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Deepo.Fetcher.Library.Fetcher.Vinyl;

internal class FetcherVinyl : CancellableWorker
{
    private readonly ILogger _logger;
    private readonly IVinylFetchPipelineFactory _pipelineFactory;

    public int FetchSucced { get; private set; }
    public int FetchFailed { get; private set; }

    public FetcherVinyl(IVinylFetchPipelineFactory pipelineFactory, ILogger logger)
    : base(logger)
    {
        _pipelineFactory = pipelineFactory;
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
        using (VinylFetchPipeline _strategy = _pipelineFactory.Create())
        {
            _strategy.OnStrategySuccess(() =>
            {
                FetchSucced++;
            });
            _strategy.OnStrategyFailure(() =>
            {
                FetchFailed++;
            });

            await _strategy.StartAsync(stoppingToken).ConfigureAwait(false);

            FetcherLogs.FetchFailed(_logger, FetchFailed);
            FetcherLogs.FetchSucceed(_logger, FetchSucced);
        }
    }
}

