using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Service.Spotify.Endpoint;
using Framework.Web.Http.Client.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Fetcher.Library.Service.Spotify;

internal class SpotifyService : AuthenticatedHttpService, ISpotifyService
{
    private readonly HttpServiceOption _options;
    private readonly EndPointSearchAlbum _searchAlbumEndpoint;

    public SpotifyService(IHttpClientFactory httpClientFactory, IOptions<HttpServicesOption> options, ILogger<SpotifyService> logger)
    : base(httpClientFactory,
        new TimeProvider(),
#pragma warning disable CA2000 // Dispose objects before losing scope
        new SpotifyAuthService(httpClientFactory, options, logger),
#pragma warning restore CA2000 // Dispose objects before losing scope
        options.Value.Spotify,
        logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Spotify, nameof(options.Value.Spotify));

        _options = options.Value.Spotify;
        _searchAlbumEndpoint = new EndPointSearchAlbum(_options, logger);
    }

    #region Public 

    public async IAsyncEnumerable<HttpServiceResult<Dto.Spotify.Album>> GetNewReleasesViaSearch(string market, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (string item in GetNewReleasesViaSearchJson(market, cancellationToken).ConfigureAwait(false))
        {
            HttpServiceResult<Dto.Spotify.Album> result = new();
            if (_searchAlbumEndpoint.TryParse(item, out IEnumerable<Dto.Spotify.Album>? parsedReleases))
            {
                if (parsedReleases is null)
                {
                    continue;
                }

                foreach (var item2 in parsedReleases.Where(x => x != null))
                {
                    result.IsSuccess = true;
                    result.Content = item2;
                    yield return result;
                }

            }
        }
    }
    #endregion

    #region GET Methods

    private async IAsyncEnumerable<string> GetNewReleasesViaSearchJson(string market, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        _searchAlbumEndpoint.Market = market;
        await foreach (var item in base.GetAsync(_searchAlbumEndpoint.Get($"year:{DateTime.Now.Year}+tag:new"), _searchAlbumEndpoint, cancellationToken).ConfigureAwait(false))
        {
            yield return item;
        }
    }
    #endregion

    protected override void SetTokenValue(string token)
    {
        base.SetAuthorization("Bearer", token);
    }
}
