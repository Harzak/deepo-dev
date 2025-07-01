using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.LogMessage;

internal static partial class FetcherLogs
{

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "{fetchName} successfully ended")]
    public static partial void Success(ILogger logger, string fetchName);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Fetch succeed: {count}")]
    public static partial void FetchSucceed(ILogger logger, int count);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Fetch failed: {count}")]
    public static partial void FetchFailed(ILogger logger, int count);

    [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "All strategies failed for release: '{title}'")]
    public static partial void AllStrategiesFailed(ILogger logger, string title);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Success: strategy {strategyName} for release: '{title}'")]
    public static partial void SuccessStrategy(ILogger logger, string strategyName, string title);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Release {id} ({title}) is ignored because already fetched (present in history)")]
    public static partial void IngoreReleaseInHistory(ILogger logger, string title, string id);

    [LoggerMessage(EventId = 6, Level = LogLevel.Error, Message = "Failed to insert release: '{title} in database | ex: {error}'")]
    public static partial void InsertFailed(ILogger logger, string title, string error);
}
