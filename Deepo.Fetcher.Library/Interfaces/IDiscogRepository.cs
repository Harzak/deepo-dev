using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Repositories;
using Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
using Framework.Common.Utils.Result;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IDiscogRepository
{
    Task<OperationResult<IEnumerable<Dto.Discogs.DtoAlbum>?>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken);
    Task<OperationResult<IEnumerable<Dto.Discogs.DtoAlbum>?>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken);
    Task<OperationResult<Dto.Discogs.DtoMaster?>> GetReleaseByID(string id, CancellationToken cancellationToken);
}
