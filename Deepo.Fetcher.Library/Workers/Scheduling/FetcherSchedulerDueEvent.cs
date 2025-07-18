using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Time;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Workers.Scheduling;

/// <summary>
/// Represents a scheduled due event that manages the timing of a specific fetcher task execution.
/// This class implements <see cref="BackgroundService"/> to efficiently handle long delays (hours to days)
/// without blocking threads, for cron-based scheduling scenarios.
/// </summary>
internal class FetcherSchedulerDueEvent : BackgroundService, IFetcherSchedulerDueEvent
{
    private readonly IDateTimeFacade _dateTimeProvider;
    private readonly ILogger _logger;
    private readonly DateTime _dueAt;
    private readonly object _eventLock;
    private bool _eventFired;

    /// <summary>
    /// Occurs when the scheduled due time is reached and the fetcher is ready for execution.
    /// This event is fired exactly once when the current UTC time reaches or exceeds the scheduled due time.
    /// </summary>
    public event EventHandler<DueEventArgs>? TriggeredEvent;

    /// <summary>
    /// Occurs when the due event is cancelled or aborted before the scheduled due time is reached.
    /// This event is fired exactly once when the task is stopped, cancelled, or encounters an error.
    /// </summary>
    public event EventHandler<DueEventArgs>? AbortedEvent;

    /// <summary>
    /// Gets the unique identifier of the fetcher associated with this due event.
    /// </summary>
    public string FetcherIdentifier { get; }

    /// <summary>
    /// Gets the unique identifier for this specific due event instance.
    /// </summary>
    public Guid EventIdentifier { get; }

    public FetcherSchedulerDueEvent(string fetcherIdentifier, DateTime dueAt, IDateTimeFacade dateTimeFacade, ILogger logger)
    {
        if (dueAt < dateTimeFacade.DateTimeUTCNow())
        {
            throw new ArgumentException("Due date must be in the future.", nameof(dueAt));
        }

        _dateTimeProvider = dateTimeFacade;
        _logger = logger;
        _dueAt = dueAt;
        FetcherIdentifier = fetcherIdentifier;
        EventIdentifier = Guid.NewGuid();
        _eventLock = new object();
    }

    /// <summary>
    /// Executes the background service logic that waits until the due time and fires the appropriate event.
    /// This method efficiently handles long delays using <see cref="Task.Delay"/> without blocking threads.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            TimeSpan delay = _dueAt - _dateTimeProvider.DateTimeUTCNow();
            SchedulerLogs.WaitForWorker(_logger, FetcherIdentifier, delay);

            if (delay <= TimeSpan.Zero)
            {
                FireTriggeredEvent();
                return;
            }

            await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            FireAbortedEvent();
            return;
        }

        FireTriggeredEvent();
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_eventFired && _dateTimeProvider.DateTimeUTCNow() < _dueAt)
        {
            FireAbortedEvent();
        }

        await base.StopAsync(cancellationToken).ConfigureAwait(false);
    }

    private void FireTriggeredEvent()
    {
        lock (_eventLock)
        {
            if (_eventFired)
            {
                return;
            }
            _eventFired = true;
            TriggeredEvent?.Invoke(this, new DueEventArgs(FetcherIdentifier, EventIdentifier, _dueAt));
        }
    }

    private void FireAbortedEvent()
    {
        lock (_eventLock)
        {
            if (_eventFired)
            {
                return;
            }
            _eventFired = true;
            AbortedEvent?.Invoke(this, new DueEventArgs(FetcherIdentifier, EventIdentifier, _dueAt));
        }
    }
}