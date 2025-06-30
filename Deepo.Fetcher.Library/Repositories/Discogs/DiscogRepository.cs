using Azure.Core;
using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Mappers;
using Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Fetcher.Library.Repositories.Discogs;

internal class DiscogRepository : HttpService, IDiscogRepository
{
    private readonly HttpServiceOption _options;

    private readonly EndPointSearch _endPointSearch;
    private readonly EndPointReleases _endPointReleases;

    public DiscogRepository(IHttpClientFactory httpClientFactory, IOptions<HttpServicesOption> options, ILogger<DiscogRepository> logger)
    : base(httpClientFactory, new TimeProvider(), options.Value.Discogs, logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Discogs, nameof(options.Value.Discogs));

        _options = options.Value.Discogs;
        _endPointSearch = new EndPointSearch(_options, logger);
        _endPointReleases = new EndPointReleases(_options, logger);

        base.SetAuthorization("Discogs", $"token={_options.Token}");
    }

    public async Task<OperationResult<IEnumerable<DtoDiscogsAlbum>?>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<DtoDiscogsAlbum>?> result = new();

        OperationResult<string> searchRequest = await this.GetSearchByReleaseTitleAndYearJson(releaseTitle, DateTime.Now.Year, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            return result.WithFailure();
        }
        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<DtoDiscogsAlbum>? parseSearch);
        if (!isSearchParsed)
        {
            return result.WithFailure();
        }
        return result.WithSuccess().WithValue(parseSearch);
    }

    private async Task<OperationResult<string>> GetSearchByReleaseTitleAndYearJson(string releaseTitle, int year, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"release_title={releaseTitle}&year={year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }

    public async Task<OperationResult<IEnumerable<DtoDiscogsAlbum>?>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<DtoDiscogsAlbum>?> result = new();

        OperationResult<string> searchRequest = await GetSearchByArtistNameAndYearJson(nameArtist, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            return result.WithFailure();
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<DtoDiscogsAlbum>? parseSearch);
        if (!isSearchParsed)
        {
            return result.WithFailure();
        }
        return result.WithSuccess().WithValue(parseSearch);
    }
    private async Task<OperationResult<string>> GetSearchByArtistNameAndYearJson(string nameArtist, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"artist={nameArtist}&year={DateTime.UtcNow.Year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }

    public async Task<OperationResult<DtoDiscogsRelease?>> GetReleaseByID(string id, CancellationToken cancellationToken)
    {
        OperationResult<DtoDiscogsRelease?> result = new();


        OperationResult<string> masterRequest = await GetReleaseByIDJson(id, cancellationToken).ConfigureAwait(false);
        if (masterRequest.IsFailed)
        {
            return result.WithFailure();
        }

        bool isReleaseParsed = _endPointReleases.TryParse(masterRequest.Content, out Dto.Discogs.DtoDiscogsRelease? parsedRelease);
        if (!isReleaseParsed || parsedRelease is null)
        {
            return result.WithFailure();
        }

        return result.WithSuccess().WithValue(parsedRelease);
    }
    private async Task<OperationResult<string>> GetReleaseByIDJson(string id, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointReleases.Get(id), cancellationToken).ConfigureAwait(false);
    }
}

