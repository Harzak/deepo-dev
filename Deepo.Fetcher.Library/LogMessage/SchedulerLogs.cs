using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.LogMessage;

internal static partial class SchedulerLogs
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Worker: {name} ({id}) is ready to start.")]
    public static partial void ReadyToStart(ILogger logger, string name, Guid id);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "Worker: {name} ({id}) is not ready to start (at {date}).")]
    public static partial void NotReadyToStart(ILogger logger, string name, Guid id, DateTime date);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Worker: {name} ({id}) next start is: {nextStart}")]
    public static partial void NextSatrt(ILogger logger, string name, Guid id, DateTime? nextStart);

    [LoggerMessage(EventId = 3, Level = LogLevel.Debug, Message = "Start evaluating ready workers ({nbWorkers})")]
    public static partial void EvaluatingReadyWorkers(ILogger logger, int nbWorkers);
}