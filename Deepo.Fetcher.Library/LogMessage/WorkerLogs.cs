using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.LogMessage;

internal static partial class WorkerLogs
{
    //Start
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Worker: {name} ({id}) try to start at {time}.")]
    public static partial void WorkerTryStart(ILogger logger, string name, Guid id, DateTime time);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Worker: {name} ({id}) started at {time}.")]
    public static partial void WorkerStarted(ILogger logger, string name, Guid id, DateTime time);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Worker: {name} ({id}) is not authorized to start.")]
    public static partial void WorkerUnhautorizedToStart(ILogger logger, string name, Guid id);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Worker: {name} ({id}) is ready to start.")]
    public static partial void WorkerIsReadyToStart(ILogger logger, string name, Guid id);

    //Stop
    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Worker: {name} ({id}) try to stop at {time}.")]
    public static partial void WorkerTryStop(ILogger logger, string name, Guid id, DateTime time);

    [LoggerMessage(EventId = 5, Level = LogLevel.Information, Message = "Worker: {name} ({id}) stopped at {time}.")]
    public static partial void WorkerStopped(ILogger logger, string name, Guid id, DateTime time);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information, Message = "Worker: {name} ({id}) cannot be stopped")]
    public static partial void WorkerCannotStop(ILogger logger, string name, Guid id);

    [LoggerMessage(EventId = 7, Level = LogLevel.Warning, Message = "Worker: {name} ({id}) was forced to stop at {time}.")]
    public static partial void WorkerForcedStopped(ILogger logger, string name, Guid id, DateTime time);

    //Finish
    [LoggerMessage(EventId = 8, Level = LogLevel.Information, Message = "Worker: {name} ({id}) achieved in {time}).")]
    public static partial void WorkerAchieved(ILogger logger, string name, Guid id, TimeSpan time);

    //Cancel
    [LoggerMessage(EventId = 9, Level = LogLevel.Warning, Message = "Worker: {name} ({id}) has been canceled {reason}")]
    public static partial void WorkerHasBeenCanceled(ILogger logger, string name, Guid id, string reason);
}