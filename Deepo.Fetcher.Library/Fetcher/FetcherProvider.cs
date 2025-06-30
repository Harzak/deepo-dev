using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule.Planification;
using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherProvider : IFetcherProvider
{
    private readonly IFetcherFactory _fetcherFactory;
    private readonly IPlanningFactory _planningFactory;
    private readonly IFetcherRepository _fetcherRepository;
    private readonly IPlanificationRepository _planificationRepository;

    public FetcherProvider(IFetcherFactory fetcherFactory,
        IPlanningFactory planningFactory,
        IFetcherRepository fetcherRepository,
        IPlanificationRepository planificationRepository)
    {
        _fetcherFactory = fetcherFactory;
        _planningFactory = planningFactory;
        _fetcherRepository = fetcherRepository;
        _planificationRepository = planificationRepository;
    }

    public IEnumerable<IWorker> GetAllFetcher()
    {
        List<IWorker> workers = [];
        ReadOnlyCollection<Models.Fetcher>? fetchersDb = _fetcherRepository.GetAll();

        if (fetchersDb is null || fetchersDb.Count == 0)
        {
            return workers;
        }

        foreach (Models.Fetcher fetcherDb in fetchersDb)
        {
            IWorker worker = _fetcherFactory.CreateFetcher(fetcherDb.Code);
            workers.Add(worker);
        }
        return workers;
    }

    public Dictionary<IWorker, IPlanning> GetAllPlannedFetcher()
    {
        Dictionary<IWorker, IPlanning> plannedWorkers = [];

        IEnumerable<Models.V_FetcherPlannification>? plannificationFetchers = _planificationRepository.GetAll();

        if (plannificationFetchers is null || !plannificationFetchers.Any())
        {
            return plannedWorkers;
        }

        foreach (Models.V_FetcherPlannification plannificationFetcher in plannificationFetchers)
        {
            IWorker worker = _fetcherFactory.CreateFetcher(plannificationFetcher.Code ?? string.Empty);
            worker.ID = Guid.Parse(plannificationFetcher.Fetcher_GUID); //todo
            IPlanning planning = _planningFactory.CreatePlanning(plannificationFetcher.PlanificationCode ?? string.Empty,
                                                                    plannificationFetcher.HourStart ?? -1,
                                                                    plannificationFetcher.MinuteStart ?? -1);
            plannedWorkers.Add(worker, planning);
        }

        return plannedWorkers;
    }

    public IWorker? GetFetcherByName(string name)
    {
        Models.Fetcher? fetcherDb = _fetcherRepository.GetByName(name);

        if (fetcherDb is null)
        {
            return null;
        }
        return _fetcherFactory.CreateFetcher(fetcherDb.Code);
    }
}
