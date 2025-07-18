using Deepo.DAL.Repository.Feature.Fetcher;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.Fetcher.Library.Fetcher;

/// <summary>
/// Manages and provides access to fetcher workers.
/// Retrieves fetcher configurations from the database and creates corresponding worker instances.
/// Acts as a factory and repository for fetcher workers.
/// </summary>
internal class FetcherProvider : IFetcherProvider
{
    private readonly IFetcherFactory _fetcherFactory;
    private readonly IFetcherRepository _fetcherRepository;
    private readonly ISchedulerRepository _planificationRepository;

    public FetcherProvider(IFetcherFactory fetcherFactory,
        IFetcherRepository fetcherRepository,
        ISchedulerRepository planificationRepository)
    {
        _fetcherFactory = fetcherFactory;
        _fetcherRepository = fetcherRepository;
        _planificationRepository = planificationRepository;
    }

    /// <summary>
    /// Retrieves all available fetcher workers from the database and creates corresponding worker instances.
    /// </summary>
    /// <returns>A collection of all available fetcher workers.</returns>
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

    /// <summary>
    /// Retrieves a specific fetcher worker by its name from the database and creates the corresponding worker instance.
    /// </summary>
    /// <param name="name">The name of the fetcher to retrieve.</param>
    /// <returns>The fetcher worker if found; otherwise, null.</returns>
    public async Task<IWorker?> GetFetcherByNameAsync(string name)
    {
        Models.Fetcher? fetcherDb = await _fetcherRepository.GetByNameAsync(name).ConfigureAwait(false);

        if (fetcherDb != null)
        {
            return _fetcherFactory.CreateFetcher(fetcherDb.Code);
        }
        return null;
    }

    /// <summary>
    /// Retrieves a specific fetcher worker by its identifier from the database and creates the corresponding worker instance.
    /// </summary>
    /// <param name="identifier">The unique identifier of the fetcher to retrieve.</param>
    /// <returns>The fetcher worker if found; otherwise, null.</returns>
    public async Task<IWorker?> GetFetcherByIdAsync(string identifier)
    {
        Models.Fetcher? fetcherDb = await _fetcherRepository.GetByIdAsync(identifier).ConfigureAwait(false);

        if (fetcherDb != null)
        {
            return _fetcherFactory.CreateFetcher(fetcherDb.Code);
        }
        return null;
    }
}
