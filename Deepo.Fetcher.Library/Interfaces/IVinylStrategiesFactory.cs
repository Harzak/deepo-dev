using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Framework.Common.Utils.Result;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IVinylStrategiesFactory
{
    Task<OperationResult<DtoDiscogsRelease>> SearchDiscogsByArtistAsync(string artistName, CancellationToken cancellationToken);
    Task<OperationResult<DtoDiscogsRelease>> SearchDiscogsByTitleAsync(string releaseTitle, CancellationToken cancellationToken);
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyFrenchMarketAsync(CancellationToken cancellationToken);
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyNorthAmericanMarketAsync(CancellationToken cancellationToken);
}