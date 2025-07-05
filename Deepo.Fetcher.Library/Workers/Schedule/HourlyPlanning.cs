using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using System.Xml.XPath;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public class HourlyPlanning : Planning
{
    private int _minuteTaskStart;
    protected int MinuteTaskStart
    {
        get => _minuteTaskStart;
        set => _minuteTaskStart = value;
    }

    public HourlyPlanning(int startMinute, ITimeProvider timeProvider, ILogger logger) : base(timeProvider, logger)
    {
        MinuteTaskStart = startMinute;
    }

    public HourlyPlanning(XPathNavigator xmlPlanning, ITimeProvider timeProvider, ILogger logger) : base(timeProvider, logger)
    {
        if (xmlPlanning != null)
        {
            string? minute_TaskStartStr = xmlPlanning.SelectSingleNode("Minute_TaskStart")?.Value;
            string? minute_TaskEndStr = xmlPlanning.SelectSingleNode("Minute_TaskEnd")?.Value;

            if (!string.IsNullOrEmpty(minute_TaskEndStr?.Trim()) && !string.IsNullOrEmpty(minute_TaskStartStr?.Trim()))
            {
                if (int.TryParse(minute_TaskStartStr, out _minuteTaskStart))
                {
                    return;
                }
            }
            throw new InvalidOperationException("Unable to read the planning parameters from XML");
        }
    }

    protected override void GetNext(DateTime datetime)
    {
        DateTime next;

        datetime = datetime.AddHours(1);
        if (MinuteTaskStart == 0)
        {
            next = datetime;
        }
        else if (datetime.Minute >= MinuteTaskStart)
        {
            next = datetime;
        }
        else
        {
            next = new DateTime(datetime.Year, datetime.Month, datetime.Day, datetime.Hour, MinuteTaskStart, datetime.Second);
        }
        NextStart = next;
    }

    protected override bool ShouldStart(DateTime startDate)
    {
        return startDate.Minute >= MinuteTaskStart;
    }
}
