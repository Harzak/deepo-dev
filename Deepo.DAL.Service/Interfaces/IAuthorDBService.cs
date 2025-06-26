using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Result;

namespace Deepo.DAL.Service.Interfaces;
public interface IAuthorDBService
{
    Task<DatabaseOperationResult> Insert(IAuthor item, CancellationToken cancellationToken);
    bool Exists(IAuthor item);
    Author GetByProviderIdentifier(string identifier);
}
