using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Result;

namespace Deepo.DAL.Repository.Interfaces;

public interface IReleaseAlbumRepository
{
    Task<DatabaseOperationResult> Insert(AlbumModel item, CancellationToken cancellationToken);
    int Count(string market);
    bool Exists(AlbumModel item);
    V_LastVinylRelease? GetLast();
    Release_Album? GetById(Guid id);
    IReadOnlyCollection<V_LastVinylRelease> GetAll();
    IReadOnlyCollection<V_LastVinylRelease> GetAll(string market, int offset, int limit);
}
