using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Utils.Time.Provider;
using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule;
using Framework.Common.Worker.Schedule.Planification;
using Microsoft.Extensions.Logging;
using ITimer = Framework.Common.Utils.Time.ITimer;

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
        Load();
        EvaluateReadyWorkers();
    }

    private void Load()
    {
        var plannedFetchers = _fetcherProvider.GetAllPlannedFetcher();
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
            _planificationRepository.UpdateDateNextStart(plannedWorker.Key.ID, dateNexStart.Value);
        }
    }

    public override bool RegisterOneShot(IWorker worker)
    {
        return base.RegisterOneShot(worker) && _planificationRepository.AddOneShot(worker);
    }

    public override bool RegisterDaily(IWorker worker, int startHour, int startMinute)
    {
        return base.RegisterDaily(worker, startHour, startMinute) && _planificationRepository.AddDaily(worker, startHour, startMinute);
    }

    public override bool RegisterHourly(IWorker worker, int startMinute)
    {
        return base.RegisterHourly(worker, startMinute) && _planificationRepository.AddHourly(worker, startMinute);
    }

    public override bool Unregister(IWorker worker)
    {
        return _planificationRepository.Delete(worker) && base.Unregister(worker);
    }
}