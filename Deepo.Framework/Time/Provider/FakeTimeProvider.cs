using Deepo.Framework.Interfaces;
namespace Deepo.Framework.Time.Provider;

public class FakeTimeProvider : ITimeProvider
{
    private readonly DateTime _dateTime;

    public FakeTimeProvider(DateTime timeToFreeze)
    {
        _dateTime = timeToFreeze;
    }

    public DateTime DateTimeNow() => _dateTime;
    public DateTimeOffset DateTimeOffsetNow() => _dateTime;
    public DateTime DateTimeUTCNow() => _dateTime;
    public DateTimeOffset DateTimeOffsetMinValue() => _dateTime;
}