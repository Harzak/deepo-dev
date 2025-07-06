using Deepo.Fetcher.Library.Authentication;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Spotify.Endpoint;
using Deepo.Fetcher.Library.Utils;
using Deepo.Framework.Time;
using Deepo.Framework.Web.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace Deepo.Fetcher.Library.Repositories.Spotify;

internal class SpotifyRepository : AuthenticatedHttpService, ISpotifyRepository
{
    private readonly HttpServiceOption _options;
    private readonly EndPointSearchAlbum _searchAlbumEndpoint;

    public SpotifyRepository(
        IHttpClientFactory httpClientFactory, 
        IAuthServiceFactory authServiceFactory,
        IOptions<HttpServicesOption> options, 
        ILogger<SpotifyRepository> logger)
    : base(httpClientFactory,
        new DateTimeFacade(),
        authServiceFactory.CreateSpotifyAuthService(),
        options.Value.Spotify ?? throw new ArgumentNullException("options.Value.SpotifyAuth"),
        logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Spotify, nameof(options.Value.Spotify));

        _options = options.Value.Spotify;
        _searchAlbumEndpoint = new EndPointSearchAlbum(_options, logger);
    }

    public async IAsyncEnumerable<HttpServiceResult<DtoSpotifyAlbum>> GetNewReleasesViaSearch(string market, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (string page in GetNewReleasesViaSearchJson(market, cancellationToken).ConfigureAwait(false))
        {
            HttpServiceResult<DtoSpotifyAlbum> result = new();

            if (_searchAlbumEndpoint.TryParse(page, out IEnumerable<DtoSpotifyAlbum>? parsedAlbumList))
            {
                if (parsedAlbumList is null)
                {
                    continue;
                }

                foreach (DtoSpotifyAlbum album in parsedAlbumList.Where(x => x != null))
                {
                    yield return result.WithSuccess().WithValue(album);
                }

            }
        }
    }

    private async IAsyncEnumerable<string> GetNewReleasesViaSearchJson(string market, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        _searchAlbumEndpoint.Market = market;
        await foreach (string item in base.GetAsync(_searchAlbumEndpoint.Get($"year:{DateTime.Now.Year}+tag:new"), _searchAlbumEndpoint, cancellationToken).ConfigureAwait(false))
        {
            yield return item;
        }
    }

    protected override void SetTokenValue(string token)
    {
        base.SetAuthorization("Bearer", token);
    }
}