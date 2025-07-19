using System.Timers;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for timer operations with configurable intervals and auto-stop functionality.
/// </summary>
public interface ITimer
{
    /// <summary>
    /// Occurs when the timer interval has elapsed.
    /// </summary>
    event ElapsedEventHandler Elapsed;
    
    /// <summary>
    /// Gets or sets the interval in milliseconds between timer events.
    /// </summary>
    double Interval { get; set; }

    /// <summary>
    /// Starts the timer to begin raising elapsed events.
    /// </summary>
    void StartTimer();
    
    /// <summary>
    /// Stops the timer and prevents further elapsed events.
    /// </summary>
    void StopTimer();
    
    /// <summary>
    /// Configures the timer to automatically stop when the specified condition function returns true.
    /// </summary>
    ITimer AutoStop(Func<bool> autoStopFunc);
}
