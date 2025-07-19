using System.Timers;

namespace Deepo.Framework.Time;

/// <summary>
/// Provides a timer implementation with configurable intervals, auto-stop functionality, and event notifications.
/// </summary>
public class Timer : Interfaces.ITimer, IDisposable
{
    /// <summary>
    /// Occurs when the timer interval has elapsed.
    /// </summary>
    public event ElapsedEventHandler? Elapsed;
    
    /// <summary>
    /// Occurs when the timer has been stopped.
    /// </summary>
    public event EventHandler? OnStop;
    
    /// <summary>
    /// Gets or sets the interval in milliseconds between timer events.
    /// </summary>
    public double Interval
    {
        get => SystemTimer.Interval;
        set => SystemTimer.Interval = value;
    }

    /// <summary>
    /// Gets the number of times the timer has ticked since it was started.
    /// </summary>
    protected int NumberOfTick { get; private set; }
    
    /// <summary>
    /// Gets the underlying system timer instance.
    /// </summary>
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

    /// <summary>
    /// Starts the timer to begin raising elapsed events.
    /// </summary>
    public virtual void StartTimer()
    {
        SystemTimer.Start();
    }

    /// <summary>
    /// Stops the timer and prevents further elapsed events.
    /// </summary>
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

    /// <summary>
    /// Configures the timer to automatically stop when the specified condition function returns true.
    /// </summary>
    /// <param name="autoStopFunc">The function that determines when to automatically stop the timer.</param>
    /// <returns>The current instance of the <see cref="Timer"/> class.</returns>
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
