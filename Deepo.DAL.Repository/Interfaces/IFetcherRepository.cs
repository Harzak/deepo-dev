using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherRepository
{
    Task<ReadOnlyCollection<Models.Fetcher>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ReadOnlyCollection<Models.V_FetcherExtended>> GetAllExtendedAsync(CancellationToken cancellationToken = default);
    Task<Models.Fetcher?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<Models.V_FetcherExtended?> GetExtendedAsync(string id, CancellationToken cancellationToken = default);
}

