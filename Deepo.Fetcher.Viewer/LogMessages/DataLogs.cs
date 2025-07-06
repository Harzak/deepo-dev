using Microsoft.Extensions.Logging;
using System;

namespace Deepo.Fetcher.Viewer.LogMessages;
    internal static partial class DataLogs
    {
        [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Unable to execute the command: {request}")]
        public static partial void UnableExecuteCommand(ILogger logger, string request, Exception ex);

        [LoggerMessage(EventId = 1, Level = LogLevel.Critical, Message = "Unable to create the command: {command}")]
        public static partial void UnableCreateCommand(ILogger logger, string command, Exception ex);

        [LoggerMessage(EventId = 2, Level = LogLevel.Critical, Message = "Unable to create the dependency on: {connStr}")]
        public static partial void UnableCreateDependency(ILogger logger, string connStr, Exception ex);

        [LoggerMessage(EventId = 3, Level = LogLevel.Critical, Message = "Unable to open the connection: {connStr}")]
        public static partial void UnableOpenConnection(ILogger logger, string connStr, Exception ex);
    }
