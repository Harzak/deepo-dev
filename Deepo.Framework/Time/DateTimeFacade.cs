using Deepo.Framework.Interfaces;

namespace Deepo.Framework.Time;

public class DateTimeFacade : IDateTimeFacade
{
    public DateTime DateTimeNow() => DateTime.Now;
    public DateTime DateTimeUTCNow() => DateTime.UtcNow;
    public DateTimeOffset DateTimeOffsetNow() => DateTimeOffset.Now;
    public DateTimeOffset DateTimeOffsetMinValue() => DateTimeOffset.MinValue;
}