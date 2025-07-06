using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Deepo.Fetcher.Library.Workers.Schedule;

public class DailyPlanning : HourlyPlanning
{
    private int _hourTaskStart;
    protected int HourTaskStart
    {
        get => _hourTaskStart;
        set => _hourTaskStart = value;
    }

    public DailyPlanning(int startHour, int startMinute, IDateTimeFacade timeProvider, ILogger logger) : base(startMinute, timeProvider, logger)
    {
        HourTaskStart = startHour;
    }

    public DailyPlanning(XPathNavigator xmlPlanning, IDateTimeFacade timeProvider, ILogger logger) : base(xmlPlanning, timeProvider, logger)
    {
        if (xmlPlanning != null)
        {
            string? hour_TaskStartStr = xmlPlanning.SelectSingleNode("Hour_TaskStart")?.Value;
            string? hour_TaskEndStr = xmlPlanning.SelectSingleNode("Hour_TaskEnd")?.Value;

            if (!string.IsNullOrEmpty(hour_TaskEndStr?.Trim()) && !string.IsNullOrEmpty(hour_TaskStartStr?.Trim()))
            {
                if (int.TryParse(hour_TaskStartStr, out _hourTaskStart))
                {
                    return;
                }
            }
            throw new InvalidOperationException("Unable to read the planning parameters from XML");
        }
    }

    protected override void GetNext(DateTime datetime)
    {
        base.GetNext(datetime);

        datetime = datetime.AddDays(1);
        if (HourTaskStart == 0)
        {
            NextStart = datetime;
        }
        else if (datetime.Hour >= HourTaskStart)
        {
            NextStart = datetime;
        }
        else
        {
            NextStart = new DateTime(datetime.Year, datetime.Month, datetime.Day, HourTaskStart, datetime.Minute, datetime.Second);
        }
    }

    protected override bool ShouldStart(DateTime startDate)
    {
        if (HourTaskStart == 0 || (startDate.Hour == HourTaskStart) || (startDate.Hour >= HourTaskStart))
        {
            return base.ShouldStart(startDate);
        }
        return false;
    }
}
