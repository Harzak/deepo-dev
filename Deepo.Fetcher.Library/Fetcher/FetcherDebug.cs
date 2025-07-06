
using Deepo.Fetcher.Library.Workers;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherDebug : CancellableWorker
{
    internal FetcherDebug(ILogger logger)
    : base(logger)
    {

    }

    protected override bool CanStop()
    {
        throw new NotImplementedException();
    }

    protected override Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine($"DEBUG FETCHER START AT: {DateTime.Now}");
        return Task.CompletedTask;
    }

    protected override void ForcedStop()
    {
        throw new NotImplementedException();
    }
}

