namespace Deepo.Framework.Interfaces;

public interface IPlanningFactory
{
    IPlanning CreatePlanning(string code, int startHour, int startMinute);
}
