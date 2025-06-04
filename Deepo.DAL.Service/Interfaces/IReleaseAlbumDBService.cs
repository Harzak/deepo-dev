using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Feature.ReleaseAlbum;
using Deepo.DAL.Service.Result;

namespace Deepo.DAL.Service.Interfaces;

public interface IReleaseAlbumDBService
{
    Task<DatabaseServiceResult> Insert(AlbumModel item, CancellationToken cancellationToken);
    int Count(string market);
    bool Exists(AlbumModel item);
    V_LastVinylRelease? GetLast();
    Release_Album? GetById(Guid id);
    IReadOnlyCollection<V_LastVinylRelease> GetAll();
    IReadOnlyCollection<V_LastVinylRelease> GetAll(string market, int offset, int limit);
}
