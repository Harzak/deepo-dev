using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule.Planification;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IFetcherProvider
{
    Task<IEnumerable<IWorker>> GetAllFetcherAsync();
    Task<Dictionary<IWorker, IPlanning>> GetAllPlannedFetcherAsync();
    Task<IWorker?> GetFetcherByNameAsync(string name);
}