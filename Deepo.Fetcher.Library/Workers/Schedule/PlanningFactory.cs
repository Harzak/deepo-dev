using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public class PlanningFactory : IPlanningFactory
{
    private readonly IDateTimeFacade _timeProvider;
    private readonly ILogger _logger;

    public PlanningFactory(IDateTimeFacade timeProvider, ILogger<PlanningFactory> logger)
    {
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public IPlanning CreatePlanning(string code, int startHour, int startMinute)
    {
        switch (code)
        {
            case "ONESHOT":
                return new OneShotPlanning(_timeProvider, _logger);
            case "DAILY":
                return new DailyPlanning(startHour, startMinute, _timeProvider, _logger);
            case "HOURLY":
                return new HourlyPlanning(startMinute, _timeProvider, _logger);
            default:
                throw new NotImplementedException();
        }
    }
}