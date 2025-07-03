using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

public class VinylStrategiesFactory : IVinylStrategiesFactory
{
    private readonly ILogger _logger;

    private readonly DiscogsSearchByArtistsStrategy _strategyDiscogsByArtists;
    private readonly DiscogsSearchByReleaseTitleStrategy _strategyDiscogsByTitle;
    private readonly SpotifyDiscoverByMarketStrategy _strategySpotifyByMarket;

    public VinylStrategiesFactory(IDiscogRepository discogRepository,  
        ISpotifyRepository spotifyRepository, 
        ILogger<VinylStrategiesFactory> logger)
    {
        _logger = logger;

        _strategyDiscogsByArtists = new DiscogsSearchByArtistsStrategy(discogRepository, logger);
        _strategyDiscogsByTitle =  new DiscogsSearchByReleaseTitleStrategy(discogRepository, logger);
        _strategySpotifyByMarket = new SpotifyDiscoverByMarketStrategy(spotifyRepository, logger);
    }

    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyFrenchMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in _strategySpotifyByMarket.DiscoverFrenchMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverSpotifyNorthAmericanMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in _strategySpotifyByMarket.DiscoverNorthAmericanMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    public async Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByArtistAsync(string artistName, CancellationToken cancellationToken)
    {
        return await _strategyDiscogsByArtists.SearchAsync(artistName, cancellationToken).ConfigureAwait(false);
    }

    public async Task<OperationResultList<DtoDiscogsRelease>> SearchDiscogsByTitleAsync(string releaseTitle, CancellationToken cancellationToken)
    {
        return await _strategyDiscogsByTitle.SearchAsync(releaseTitle, cancellationToken).ConfigureAwait(false);
    }
}
