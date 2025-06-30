using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Service;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IDiscogService
{
    Task<HttpServiceResult<AlbumModel>> GetReleaseByArtist(string nameArtist, CancellationToken cancellationToken);
    Task<HttpServiceResult<AlbumModel>> GetReleaseByName(string releaseTitle, CancellationToken cancellationToken);
}
