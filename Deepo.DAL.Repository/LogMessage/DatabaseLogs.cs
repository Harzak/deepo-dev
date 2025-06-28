using Microsoft.Extensions.Logging;

namespace Deepo.DAL.Repository.LogMessage;

internal static partial class DatabaseLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Unable to add the item {item} in: {connstring}.")]
    public static partial void UnableToAdd(ILogger logger, string item, string? connstring, Exception ex);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Unable to remove the item {item} in: {connstring}.")]
    public static partial void UnableToRemove(ILogger logger, string item, string? connstring, Exception ex);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Unable to update the item {item} in: {connstring}.")]
    public static partial void UnableToUpdate(ILogger logger, string item, string? connstring, Exception ex);
}
