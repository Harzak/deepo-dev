using Deepo.DAL.EF.Models;
using Framework.Common.Worker.Interfaces;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherExecutionRepository
{
    bool LogStart(IWorker worker);

    bool LogEnd(IWorker worker);

    IEnumerable<V_FetchersLastExecution> GetFetcherExecutions();
}

