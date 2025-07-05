
namespace Deepo.Framework.Interfaces;

public interface ITimeProvider
{
    DateTime DateTimeNow();
    DateTime DateTimeUTCNow();
    DateTimeOffset DateTimeOffsetNow();
    DateTimeOffset DateTimeOffsetMinValue();
}