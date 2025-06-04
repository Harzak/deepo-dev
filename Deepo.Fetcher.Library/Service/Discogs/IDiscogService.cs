using Deepo.DAL.Service.Feature.ReleaseAlbum;
using Deepo.DAL.Service.Interfaces;

namespace Deepo.Fetcher.Library.Service.Discogs;

public interface IDiscogService
{
    Task<HttpServiceResult<IAuthor>> GetArtist(string nameArtist, CancellationToken cancellationToken);
    Task<HttpServiceResult<AlbumModel>> GetLastReleaseByArtistID(string id, CancellationToken cancellationToken);
}
