using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Strategies.Vinyl;
using Deepo.Fetcher.Library.Utils;
using Deepo.Fetcher.Library.Workers;
using Deepo.Framework.Results;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Deepo.Fetcher.Library.Fetcher.Vinyl;

/// <summary>
/// Vinyl-specific fetcher implementation that processes vinyl record data.
/// Uses a vinyl fetch pipeline to discover and process vinyl releases from various sources.
/// </summary>
internal sealed class FetcherVinyl : CancellableWorker
{
    private readonly ILogger _logger;
    private readonly IVinylFetchPipelineFactory _pipelineFactory;

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

    /// <summary>
    /// Executes the vinyl fetcher's main operation asynchronously.
    /// Creates a vinyl fetch pipeline and processes vinyl data, logging the results.
    /// </summary>
    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        using VinylFetchPipeline _strategy = _pipelineFactory.Create();

        await _strategy.StartAsync(stoppingToken).ConfigureAwait(false);

        FetcherLogs.FetchIgnored(_logger, _strategy.IgnoredFetchCount, _strategy.FetchCount);
        FetcherLogs.FetchFailed(_logger, _strategy.FailedFetchCount, _strategy.FetchCount);
        FetcherLogs.FetchSucceed(_logger, _strategy.SuccessfulFetchCount, _strategy.FetchCount);
    }
}

