using Deepo.DAL.Service.Result;

namespace Deepo.DAL.Service.Interfaces;
public interface IAuthorDBService
{
    Task<DatabaseServiceResult> Insert(IAuthor item, CancellationToken cancellationToken);

    bool Exists(IAuthor item);
}
