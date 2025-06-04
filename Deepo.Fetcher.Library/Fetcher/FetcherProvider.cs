using Deepo.DAL.Service.Feature.Fetcher;
using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule.Planification;
using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Library.Fetcher;

internal class FetcherProvider : IFetcherProvider
{
    private readonly IFetcherFactory _fetcherFactory;
    private readonly IPlanningFactory _planningFactory;
    private readonly IFetcherDBService _fetcherdbservice;
    private readonly IPlanificationDBService _planificationDBService;

    public FetcherProvider(IFetcherFactory fetcherFactory,
        IPlanningFactory planningFactory,
        IFetcherDBService fetcherdbservice,
        IPlanificationDBService planificationDBService)
    {
        _fetcherFactory = fetcherFactory;
        _planningFactory = planningFactory;
        _fetcherdbservice = fetcherdbservice;
        _planificationDBService = planificationDBService;
    }

    public IEnumerable<IWorker> GetAllFetcher()
    {
        List<IWorker> workers = [];
        ReadOnlyCollection<Models.Fetcher>? fetchersDb = _fetcherdbservice.GetAll();

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

        IEnumerable<Models.V_FetcherPlannification>? plannificationFetchers = _planificationDBService.GetAll();

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
        Models.Fetcher? fetcherDb = _fetcherdbservice.GetByName(name);

        if (fetcherDb is null)
        {
            return null;
        }
        return _fetcherFactory.CreateFetcher(fetcherDb.Code);
    }
}
