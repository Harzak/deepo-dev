using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.LogMessage;
internal static partial class FetcherLogs
{

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "{fetchName} successfully ended")]
    public static partial void Success(ILogger logger, string fetchName);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Fetch succeed: {count}")]
    public static partial void FetchSucceed(ILogger logger, int count);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Fetch failed: {count}")]
    public static partial void FetchFailed(ILogger logger, int count);
}
