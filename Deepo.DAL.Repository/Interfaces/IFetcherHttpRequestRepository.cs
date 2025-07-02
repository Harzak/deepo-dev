using Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherHttpRequestRepository
{
    Task<HttpRequest?> GetLastAsync(CancellationToken cancellationToken = default);
}
