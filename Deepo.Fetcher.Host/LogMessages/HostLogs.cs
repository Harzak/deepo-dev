using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Host.LogMessages;

internal static partial class HostLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Host: {type} started at {time}.")]
    public static partial void HostStarted(ILogger logger, Type type, DateTime time);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "Host: {type} stopped at {time}.")]
    public static partial void HostStopped(ILogger logger, Type type, DateTime time);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Host: {type} was forced to stop at {time}.")]
    public static partial void HostForcedStopped(ILogger logger, Type type, DateTime time);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Host: {type} achieved in {time}).")]
    public static partial void HostAchieved(ILogger logger, Type type, TimeSpan time);

    [LoggerMessage(EventId = 4, Level = LogLevel.Information, Message = "Host: {type} cannot be stopped")]
    public static partial void HostCannotStop(ILogger logger, Type type);
}