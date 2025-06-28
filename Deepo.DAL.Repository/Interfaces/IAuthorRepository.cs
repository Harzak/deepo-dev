using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Result;

namespace Deepo.DAL.Repository.Interfaces;
public interface IAuthorRepository
{
    Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken);
    bool Exists(IAuthor item);
    Author GetByProviderIdentifier(string identifier);
}
