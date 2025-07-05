using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Framework.Results;
using System.Collections.ObjectModel;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IVinylStrategiesFactory
{
    Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByArtistAsync(string artistName, CancellationToken cancellationToken);
    Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByTitleAsync(string releaseTitle, CancellationToken cancellationToken);
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyFrenchMarketAsync(CancellationToken cancellationToken);
    IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyNorthAmericanMarketAsync(CancellationToken cancellationToken);
}