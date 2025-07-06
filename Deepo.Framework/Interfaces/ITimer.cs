using System.Timers;

namespace Deepo.Framework.Interfaces;

public interface ITimer
{
    event ElapsedEventHandler Elapsed;
    double Interval { get; set; }

    void StartTimer();
    void StopTimer();
    ITimer AutoStop(Func<bool> autoStopFunc);
}
