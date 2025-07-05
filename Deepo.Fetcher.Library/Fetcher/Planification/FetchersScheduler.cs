using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Workers.Schedule;
using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;
using ITimer = Deepo.Framework.Interfaces.ITimer;

namespace Deepo.Fetcher.Library.Fetcher.Planification;

public class FetchersScheduler : Scheduler
{
    private readonly IPlanificationRepository _planificationRepository;
    private readonly IFetcherProvider _fetcherProvider;

    public FetchersScheduler(IFetcherProvider fetcherProvider,
        IPlanificationRepository planificationRepository,
        ITimer timer,
        ITimeProvider datetimeprovider,
        ILogger<FetchersScheduler> logger)
    : base(datetimeprovider, timer, logger)
    {
        _fetcherProvider = fetcherProvider;
        _planificationRepository = planificationRepository;
    }

    public override void Start()
    {
        base.Start();
        LoadAsync().Wait();
        EvaluateReadyWorkers();
    }

    private async Task LoadAsync()
    {
        var plannedFetchers = await _fetcherProvider.GetAllPlannedFetcherAsync().ConfigureAwait(false);
        foreach (KeyValuePair<IWorker, IPlanning> plannedFetcher in plannedFetchers)
        {
            UpSertWorker(plannedFetcher.Key, plannedFetcher.Value);
        }
    }

    protected override void EvaluateReadyWorker(KeyValuePair<IWorker, IPlanning> plannedWorker)
    {
        base.EvaluateReadyWorker(plannedWorker);
        DateTime? dateNexStart = plannedWorker.Value.DateNextStart;
        if (dateNexStart.HasValue)
        {
            _planificationRepository.UpdateDateNextStartAsync(plannedWorker.Key.ID, dateNexStart.Value).Wait();
        }
    }

    public override bool RegisterOneShot(IWorker worker)
    {
        return base.RegisterOneShot(worker) && _planificationRepository.AddOneShotAsync(worker).Result;
    }

    public override bool RegisterDaily(IWorker worker, int startHour, int startMinute)
    {
        return base.RegisterDaily(worker, startHour, startMinute) && _planificationRepository.AddDailyAsync(worker, startHour, startMinute).Result;
    }

    public override bool RegisterHourly(IWorker worker, int startMinute)
    {
        return base.RegisterHourly(worker, startMinute) && _planificationRepository.AddHourlyAsync(worker, startMinute).Result;
    }

    public override bool Unregister(IWorker worker)
    {
        return _planificationRepository.DeleteAsync(worker).Result && base.Unregister(worker);
    }
}