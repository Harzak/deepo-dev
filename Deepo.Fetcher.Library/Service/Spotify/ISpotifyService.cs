namespace Deepo.Fetcher.Library.Service.Spotify;

public interface ISpotifyService
{
    IAsyncEnumerable<HttpServiceResult<Dto.Spotify.Album>> GetNewReleasesViaSearch(CancellationToken cancellationToken);
}
