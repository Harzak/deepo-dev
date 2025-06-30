using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Repositories;
using Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
using Framework.Common.Utils.Result;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IDiscogRepository
{
    Task<OperationResult<IEnumerable<Dto.Discogs.DtoDiscogsAlbum>?>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken);
    Task<OperationResult<IEnumerable<Dto.Discogs.DtoDiscogsAlbum>?>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken);
    Task<OperationResult<Dto.Discogs.DtoDiscogsRelease?>> GetReleaseByID(string id, CancellationToken cancellationToken);
}
