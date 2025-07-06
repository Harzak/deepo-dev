using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public class OneShotPlanning : Planning
{
    public OneShotPlanning(IDateTimeFacade timeProvider, ILogger pLogger) : base(timeProvider, pLogger)
    {

    }

    protected override void GetNext(DateTime datetime)
    {
        NextStart = DateTime.MinValue;
    }
    protected override bool ShouldStart(DateTime startDate)
    {
        if (NextStart.HasValue && NextStart.Value == DateTime.MinValue)
        {
            return false;
        }
        return true;
    }
}