using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IFetcherProvider
{
    Task<IEnumerable<IWorker>> GetAllFetcherAsync();
    Task<Dictionary<IWorker, IPlanning>> GetAllPlannedFetcherAsync();
    Task<IWorker?> GetFetcherByNameAsync(string name);
}