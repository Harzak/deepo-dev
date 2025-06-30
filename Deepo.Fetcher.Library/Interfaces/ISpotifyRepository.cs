using Deepo.Fetcher.Library.Repositories;

namespace Deepo.Fetcher.Library.Interfaces;

public interface ISpotifyRepository
{
    IAsyncEnumerable<HttpServiceResult<Dto.Spotify.DtoSpotifyAlbum>> GetNewReleasesViaSearch(string market, CancellationToken cancellationToken);
}
