using Deepo.Fetcher.Library.Service;

namespace Deepo.Fetcher.Library.Interfaces;

public interface ISpotifyService
{
    IAsyncEnumerable<HttpServiceResult<Dto.Spotify.Album>> GetNewReleasesViaSearch(string market, CancellationToken cancellationToken);
}
