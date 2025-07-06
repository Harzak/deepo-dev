
using System.Timers;

namespace Deepo.Framework.Time;

public class Timer : Interfaces.ITimer, IDisposable
{
    public event ElapsedEventHandler? Elapsed;
    public event EventHandler? OnStop;
    public double Interval
    {
        get => SystemTimer.Interval;
        set => SystemTimer.Interval = value;
    }

    protected int NumberOfTick { get; private set; }
    protected System.Timers.Timer SystemTimer { get; }

    private Func<bool>? _autoStop;

    public Timer(double interval, bool autoReset = true)
    {
        SystemTimer = new System.Timers.Timer()
        {
            AutoReset = autoReset
        };
        Interval = interval;
        SystemTimer.Elapsed += SystemTimer_Elapsed;
    }

    private void SystemTimer_Elapsed(object? sender, ElapsedEventArgs e)
    {
        NumberOfTick++;

        if (ShouldStop())
        {
            StopTimer();
            return;
        }

        if (ShouldRaise())
        {
            Elapsed?.Invoke(sender, e);
        }
    }

    protected virtual bool ShouldRaise()
    {
        return SystemTimer.Enabled;
    }

    public virtual void StartTimer()
    {
        SystemTimer.Start();
    }

    public virtual void StopTimer()
    {
        SystemTimer.Stop();
        if (OnStop is null)
        {
            return;
        }
        OnStop.Invoke(this, EventArgs.Empty);
    }

    protected virtual bool ShouldStop()
    {
        return _autoStop != null && _autoStop.Invoke();
    }

    public Interfaces.ITimer AutoStop(Func<bool> autoStopFunc)
    {
        ArgumentNullException.ThrowIfNull(autoStopFunc);

        _autoStop = autoStopFunc;
        return this;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            StopTimer();
            SystemTimer.Elapsed -= SystemTimer_Elapsed;
            SystemTimer?.Dispose();
        }
    }
}
