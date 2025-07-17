using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Framework.EventArguments;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using NCrontab;
using System.Timers;
using ITimer = Deepo.Framework.Interfaces.ITimer;

namespace Deepo.Fetcher.Library.Fetcher;

/// <summary>
/// Manages the scheduling and execution of fetcher workers based on cron expressions.
/// Evaluates fetcher schedules periodically and triggers worker execution when their scheduled time arrives.
/// </summary>
public sealed class FetchersScheduler : IScheduler, IDisposable
{
    private readonly ILogger _logger;
    private readonly ITimer _timer;
    private readonly IDateTimeFacade _datetimeprovider;
    private readonly ISchedulerRepository _schedulerRepository;
    private readonly IFetcherProvider _fetcherProvider;

    private readonly CrontabSchedule.ParseOptions _crontParseOptions;
    private readonly ElapsedEventHandler _timerElapsedEventHandler;
    private readonly TimeSpan _timerInterval = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Event triggered when a worker is ready to start based on its schedule.
    /// </summary>
    public event EventHandler<WorkerEventArgs>? ReadyToStart;

    public FetchersScheduler(ISchedulerRepository schedulerRepository,
        IFetcherProvider fetcherProvider,
        ITimer timer,
        IDateTimeFacade datetimeprovider,
        ILogger<FetchersScheduler> logger)
    {
        _schedulerRepository = schedulerRepository;
        _fetcherProvider = fetcherProvider;
        _timer = timer;
        _datetimeprovider = datetimeprovider;
        _logger = logger;
        _crontParseOptions = new CrontabSchedule.ParseOptions
        {
            IncludingSeconds = false
        };
        _timerElapsedEventHandler = async (sender, e) => await EvaluateReadyFetcherAsync(CancellationToken.None).ConfigureAwait(false);
    }

    /// <summary>
    /// Starts the scheduler asynchronously by configuring the timer and performing an initial schedule evaluation.
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _timer.Elapsed += _timerElapsedEventHandler;
        _timer.Interval = _timerInterval.TotalMilliseconds;
        _timer.StartTimer();
        await EvaluateReadyFetcherAsync(cancellationToken).ConfigureAwait(false);
    }

    private async Task EvaluateReadyFetcherAsync(CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> fetcherSchedules = await _schedulerRepository.GetAllFetcherCronExpressionAsync(cancellationToken).ConfigureAwait(false);

        SchedulerLogs.EvaluatingReadyWorkers(_logger, fetcherSchedules.Count);

        foreach (KeyValuePair<string, string> schedule in fetcherSchedules.Where(x => !string.IsNullOrEmpty(x.Key) && !string.IsNullOrEmpty(x.Value)))
        {
            await EvaluateReadyWorkerAsync(schedule).ConfigureAwait(false);
        }
    }

    private async Task EvaluateReadyWorkerAsync(KeyValuePair<string, string> schedule)
    {
        CrontabSchedule crontTab = CrontabSchedule.TryParse(schedule.Value, _crontParseOptions);
        if (crontTab == null)
        {
            return;
        }

        DateTime utcNow = _datetimeprovider.DateTimeUTCNow();
        DateTime nextOccurence = crontTab.GetNextOccurrence(utcNow);
        if (nextOccurence <= utcNow)
        {
            SchedulerLogs.ReadyToStart(_logger, schedule.Key, utcNow);
            IWorker? worker = await _fetcherProvider.GetFetcherByIdAsync(schedule.Key).ConfigureAwait(false);
            if (worker != null)
            {
                ReadyToStart?.Invoke(this, new WorkerEventArgs(worker));
            }

        }
        else
        {
            SchedulerLogs.NotReadyToStart(_logger, schedule.Key, utcNow);
        }
        // ????? _schedulerRepository.UpdateDateNextStartAsync(schedule.Key.ID, dateNexStart.Value).Wait();   
    }

    public void Dispose()
    {
        _timer.Elapsed -= _timerElapsedEventHandler;
    }
}