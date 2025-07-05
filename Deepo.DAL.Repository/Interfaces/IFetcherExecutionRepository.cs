using Deepo.DAL.EF.Models;
using Deepo.Framework.Interfaces;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherExecutionRepository
{
    Task<bool> LogStartAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<bool> LogEndAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<IEnumerable<V_FetchersLastExecution>> GetFetcherExecutionsAsync(CancellationToken cancellationToken = default);
    Task<V_FetchersLastExecution?> GetLastFetcherExecutionAsync(CancellationToken cancellationToken = default);
}