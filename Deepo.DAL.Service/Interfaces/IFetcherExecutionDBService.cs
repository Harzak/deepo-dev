using Deepo.DAL.EF.Models;
using Framework.Common.Worker.Interfaces;

namespace Deepo.DAL.Service.Feature.Fetcher;

public interface IFetcherExecutionDBService
{
    bool LogStart(IWorker worker);

    bool LogEnd(IWorker worker);

    IEnumerable<V_FetchersLastExecution> GetFetcherExecutions();
}

