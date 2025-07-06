
namespace Deepo.Framework.Interfaces;

public interface IDateTimeFacade
{
    DateTime DateTimeNow();
    DateTime DateTimeUTCNow();
    DateTimeOffset DateTimeOffsetNow();
    DateTimeOffset DateTimeOffsetMinValue();
}