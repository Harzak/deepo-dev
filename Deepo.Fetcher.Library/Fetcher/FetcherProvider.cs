using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
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

    public async Task<IEnumerable<IWorker>> GetAllFetcherAsync()
    {
        List<IWorker> workers = [];
        ReadOnlyCollection<Models.Fetcher>? fetchersDb = await _fetcherRepository.GetAllAsync().ConfigureAwait(false);

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

    public async Task<Dictionary<IWorker, IPlanning>> GetAllPlannedFetcherAsync()
    {
        Dictionary<IWorker, IPlanning> plannedWorkers = [];

        IEnumerable<Models.V_FetcherPlannification>? plannificationFetchers =  await _planificationRepository.GetAllAsync().ConfigureAwait(false);

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

    public async Task<IWorker?> GetFetcherByNameAsync(string name)
    {
        Models.Fetcher? fetcherDb = await _fetcherRepository.GetByNameAsync(name).ConfigureAwait(false);

        if (fetcherDb != null)
        {
            return _fetcherFactory.CreateFetcher(fetcherDb.Code);
        }
        return null;
    }
}
