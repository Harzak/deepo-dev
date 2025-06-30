using Deepo.Fetcher.Library.Dto.Discogs;
using Framework.Common.Utils.Result;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IDiscogRepository
{
    Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken);
    Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken);
    Task<OperationResult<DtoDiscogsRelease>> GetReleaseByID(string id, CancellationToken cancellationToken);
}
