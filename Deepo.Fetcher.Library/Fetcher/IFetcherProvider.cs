using Framework.Common.Worker.Interfaces;
using Framework.Common.Worker.Schedule.Planification;

namespace Deepo.Fetcher.Library.Fetcher;

public interface IFetcherProvider
{
    IEnumerable<IWorker> GetAllFetcher();
    Dictionary<IWorker, IPlanning> GetAllPlannedFetcher();
    IWorker? GetFetcherByName(string name);
}
