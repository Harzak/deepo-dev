using Deepo.DAL.Repository.Result;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

public interface IAuthorRepository
{
    Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(IAuthor item, CancellationToken cancellationToken = default);
    Task<Models.Author?> GetByProviderIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
}
