﻿using Deepo.Fetcher.Library.Interfaces;
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
        using VinylFetchPipeline _strategy = _pipelineFactory.Create();

        await _strategy.StartAsync(stoppingToken).ConfigureAwait(false);

        FetcherLogs.FetchIgnored(_logger, _strategy.IgnoredFetchCount, _strategy.FetchCount);
        FetcherLogs.FetchFailed(_logger, _strategy.FailedFetchCount, _strategy.FetchCount);
        FetcherLogs.FetchSucceed(_logger, _strategy.SuccessfulFetchCount, _strategy.FetchCount);
    }
}

