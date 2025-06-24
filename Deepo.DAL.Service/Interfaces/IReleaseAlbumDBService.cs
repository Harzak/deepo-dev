using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Feature.Release;
using Deepo.DAL.Service.Result;

namespace Deepo.DAL.Service.Interfaces;

public interface IReleaseAlbumDBService
{
    Task<DatabaseOperationResult> Insert(AlbumModel item, CancellationToken cancellationToken);
    int Count(string market);
    bool Exists(AlbumModel item);
    V_LastVinylRelease? GetLast();
    Release_Album? GetById(Guid id);
    IReadOnlyCollection<V_LastVinylRelease> GetAll();
    IReadOnlyCollection<V_LastVinylRelease> GetAll(string market, int offset, int limit);
}
