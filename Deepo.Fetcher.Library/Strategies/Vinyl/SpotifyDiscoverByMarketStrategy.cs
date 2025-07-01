using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

public class SpotifyDiscoverByMarketStrategy 
{
    private const string MARKET_FR = "FR";
    private const string MARKET_US = "US";

    private readonly ILogger _logger;
    private readonly ISpotifyRepository _spotifyRepository;

    public SpotifyDiscoverByMarketStrategy(ISpotifyRepository spotifyRepository, ILogger logger)
    {
        _logger = logger;
        _spotifyRepository = spotifyRepository;
    }

    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverFrenchMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in DiscoverByMarketAsync(MARKET_FR, cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    public async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverNorthAmericanMarketAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var album in DiscoverByMarketAsync(MARKET_US, cancellationToken).ConfigureAwait(false))
        {
            yield return album;
        }
    }

    private async IAsyncEnumerable<DtoSpotifyAlbum> DiscoverByMarketAsync(string market, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (HttpServiceResult<DtoSpotifyAlbum> resultAlbum in _spotifyRepository.GetNewReleasesViaSearch(market, cancellationToken).ConfigureAwait(false))
        {
            if (resultAlbum.IsSuccess 
                && resultAlbum.Content?.Artists?.Any(x => x?.Name != null) == true 
                && !string.IsNullOrEmpty(resultAlbum.Content?.Name)
                && resultAlbum.Content.Id != null)
            {
                VinylStrategyLogs.SuccessSpotifyReleaseDiscovery(_logger, string.Join(", ", resultAlbum.Content.Artists.Select(x => x.Name)), resultAlbum.Content.Name);
                yield return resultAlbum.Content;
            }
            else
            {
                VinylStrategyLogs.FailedSpotifyReleaseDiscovery(_logger, resultAlbum.ErrorCode, resultAlbum.ErrorMessage);
                continue;
            }
        }
    }
}
