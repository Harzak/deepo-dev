using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using NCrontab;
using System.Collections.Concurrent;

namespace Deepo.Fetcher.Library.Workers.Scheduling;

/// <summary>
/// Manages the scheduling of fetcher workers based on cron expressions.
/// This scheduler evaluates cron schedules and triggers worker execution when their scheduled time arrives.
/// </summary>
public sealed class FetcherScheduler : IScheduler, IDisposable
{
    private readonly ILogger _logger;
    private readonly IDateTimeFacade _datetimeprovider;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IFetcherProvider _fetcherProvider;
    private readonly IFetcherSchedulerDueEventFactory _dueEventFactory;

    private readonly Dictionary<string, IFetcherSchedulerDueEvent> _dueEvents;
    private readonly SemaphoreSlim _operationSemaphore;
    private readonly CrontabSchedule.ParseOptions _crontParseOptions;

    /// <summary>
    /// Event triggered when a worker is ready to start based on its schedule.
    /// Subscribers to this event should handle the actual execution of the worker.
    /// </summary>
    public event EventHandler<WorkerEventArgs>? ReadyToStart;

    public FetcherScheduler(ISchedulerRepository schedulerRepository,
        IFetcherProvider fetcherProvider,
        IFetcherSchedulerDueEventFactory dueEventFactory,
        IDateTimeFacade datetimeprovider,
        ILogger<FetcherScheduler> logger)
    {
        _schedulerRepository = schedulerRepository;
        _fetcherProvider = fetcherProvider;
        _dueEventFactory = dueEventFactory;
        _datetimeprovider = datetimeprovider;
        _logger = logger;

        _crontParseOptions = new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = false
        };
        _dueEvents = [];
        _operationSemaphore = new SemaphoreSlim(1, 1);
    }

    /// <summary>
    /// Starts the scheduler asynchronously by loading all fetcher schedules and creating due events for their next occurrences.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> fetcherSchedules = await _schedulerRepository.GetAllFetcherCronExpressionAsync(cancellationToken).ConfigureAwait(false);

        SchedulerLogs.EvaluatingReadyWorkers(_logger, fetcherSchedules.Count);

        IEnumerable<Task> schedulingTasks = fetcherSchedules
                                                .Where(IsValidSchedule)
                                                .Select(schedule => ScheduleFetcherAsync(schedule, cancellationToken));

        await Task.WhenAll(schedulingTasks).ConfigureAwait(false);
    }

    private static bool IsValidSchedule(KeyValuePair<string, string> schedule)
    {
        return !string.IsNullOrWhiteSpace(schedule.Key) && !string.IsNullOrWhiteSpace(schedule.Value);
    }

    private async Task ScheduleFetcherAsync(KeyValuePair<string, string> schedule, CancellationToken cancellationToken)
    {
        DateTime? nextOccurrence = GetNextOccurrenceFromCrontab(schedule.Value);
        if (nextOccurrence.HasValue)
        {
            SchedulerLogs.NextStart(_logger, schedule.Key, nextOccurrence.Value);
            await AddDueEventAsync(nextOccurrence.Value, schedule.Key, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task ScheduleNextOccurrenceAsync(string fetcherIdentifier)
    {
        string? crontab = await _schedulerRepository.GetCronExpressionForFectherAsync(fetcherIdentifier, CancellationToken.None).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(crontab))
        {
            DateTime? nextOccurrence = GetNextOccurrenceFromCrontab(crontab);
            if (nextOccurrence.HasValue)
            {
                SchedulerLogs.NextStart(_logger, fetcherIdentifier, nextOccurrence.Value);
                await AddDueEventAsync(nextOccurrence.Value, fetcherIdentifier, CancellationToken.None).ConfigureAwait(false);
            }
        }
    }

    private async Task AddDueEventAsync(DateTime dueAt, string fetcherIdentifier, CancellationToken cancellationToken = default)
    {
        await _operationSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            IFetcherSchedulerDueEvent dueEvent = _dueEventFactory.Create(fetcherIdentifier, dueAt);
            dueEvent.AbortedEvent += OnDueEventAborted;
            dueEvent.TriggeredEvent += OnDueEventTriggered;

            await dueEvent.StartAsync(cancellationToken).ConfigureAwait(false);
            _dueEvents[fetcherIdentifier] = dueEvent;
        }
        finally
        {
            _operationSemaphore.Release();
        }
    }

    private async Task RemoveDueEventAsync(string fetcherIdentifier)
    {
        await _operationSemaphore.WaitAsync(CancellationToken.None).ConfigureAwait(false);

        try
        {
            if (_dueEvents.TryGetValue(fetcherIdentifier, out var dueEvent))
            {
                _dueEvents.Remove(fetcherIdentifier);

                dueEvent.AbortedEvent -= OnDueEventAborted;
                dueEvent.TriggeredEvent -= OnDueEventTriggered;

                await dueEvent.StopAsync(CancellationToken.None).ConfigureAwait(false);
            }
        }
        finally
        {
            _operationSemaphore.Release();
        }

    }

    private async void OnDueEventTriggered(object? sender, DueEventArgs e)
    {
        SchedulerLogs.ReadyToStart(_logger, e.FetcherIdentifier, e.DueAt);

        IWorker? worker = await _fetcherProvider.GetFetcherByIdAsync(e.FetcherIdentifier).ConfigureAwait(false);
        if (worker != null)
        {
            ReadyToStart?.Invoke(this, new WorkerEventArgs(worker));
        }

        await RemoveDueEventAsync(e.FetcherIdentifier).ConfigureAwait(false);
        await ScheduleNextOccurrenceAsync(e.FetcherIdentifier).ConfigureAwait(false);
    }

    private async void OnDueEventAborted(object? sender, DueEventArgs e)
    {
        await RemoveDueEventAsync(e.FetcherIdentifier).ConfigureAwait(false);
    }

    private DateTime? GetNextOccurrenceFromCrontab(string cronTabExpression)
    {
        CrontabSchedule crontTab = CrontabSchedule.TryParse(cronTabExpression, _crontParseOptions);
        if (crontTab == null)
        {
            return null;
        }

        DateTime utcNow = _datetimeprovider.DateTimeUTCNow();
        return crontTab.GetNextOccurrence(utcNow);
    }

    public void Dispose()
    {
        try
        {
            _operationSemaphore.Wait(TimeSpan.FromSeconds(5));
            try
            {
                var stopTasks = _dueEvents.Values.Select(async dueEvent =>
                {
                    dueEvent.AbortedEvent -= OnDueEventAborted;
                    dueEvent.TriggeredEvent -= OnDueEventTriggered;
                    await dueEvent.StopAsync(CancellationToken.None).ConfigureAwait(false);
                });

                Task.WaitAll(stopTasks.ToArray(), TimeSpan.FromSeconds(2));
                _dueEvents.Clear();
            }
            finally
            {
                _operationSemaphore.Release();
            }
        }
        finally
        {
            _operationSemaphore.Dispose();
        }
    }
}