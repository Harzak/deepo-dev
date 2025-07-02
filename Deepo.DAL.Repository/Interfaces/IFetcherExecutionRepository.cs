using Deepo.DAL.EF.Models;
using Framework.Common.Worker.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherExecutionRepository
{
    Task<bool> LogStartAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<bool> LogEndAsync(IWorker worker, CancellationToken cancellationToken = default);
    Task<IEnumerable<V_FetchersLastExecution>> GetFetcherExecutionsAsync(CancellationToken cancellationToken = default);
    Task<V_FetchersLastExecution?> GetLastFetcherExecutionAsync(CancellationToken cancellationToken = default);
}