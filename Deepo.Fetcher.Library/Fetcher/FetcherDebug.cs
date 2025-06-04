
using Framework.Common.Worker;
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
#pragma warning disable CA1303 // Do not pass literals as localized parameters
        Console.WriteLine("*****************************");
        Console.WriteLine($"DEBUG FETCHER START AT: {DateTime.Now}");
        Console.WriteLine("*****************************");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
        return Task.CompletedTask;
    }

    protected override void ForcedStop()
    {
        throw new NotImplementedException();
    }
}

