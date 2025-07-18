using Deepo.Fetcher.Library.Workers;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher;

/// <summary>
/// A debug implementation of a fetcher worker used for testing and debugging purposes.
/// Provides a simple console output when executed without performing actual data fetching operations.
/// </summary>
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

