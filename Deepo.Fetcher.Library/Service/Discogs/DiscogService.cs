using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Mappers;
using Deepo.Fetcher.Library.Service.Discogs.Endpoint;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Fetcher.Library.Service.Discogs;

internal class DiscogService : HttpService, IDiscogService
{
    private readonly HttpServiceOption _options;

    private readonly EndPointSearch _endPointSearch;
    private readonly EndPointReleases _endPointReleases;
    private readonly EndPointArtistReleases _endPointArtistReleases;

    public DiscogService(IHttpClientFactory httpClientFactory, IOptions<HttpServicesOption> options, ILogger<DiscogService> logger)
    : base(httpClientFactory, new TimeProvider(), options.Value.Discogs, logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Discogs, nameof(options.Value.Discogs));

        _options = options.Value.Discogs;
        _endPointSearch = new EndPointSearch(_options, logger);
        _endPointReleases = new EndPointReleases(_options, logger);
        _endPointArtistReleases = new EndPointArtistReleases(_options, logger);

        base.SetAuthorization("Discogs", $"token={_options.Token}");
    }

    #region Public 
    public async Task<HttpServiceResult<AlbumModel>> GetReleaseByArtist(string nameArtist, CancellationToken cancellationToken)
    {
        HttpServiceResult<AlbumModel> result = new();

        if (string.IsNullOrEmpty(nameArtist?.Trim()))
        {
            return result.WithError("Invalid id");
        }

        OperationResult<string> searchRequest = await GetSearchByArtistNameAndYearJson(nameArtist, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            result.Affect(searchRequest);
            return result;
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<Dto.Discogs.DtoAlbum>? parseSearch);
        if (!isSearchParsed)
        {
            return result.WithFailure();
        }

        if (parseSearch == null || parseSearch.Any(x => x != null && x?.Id != 0) == false)
        {
            return result.WithFailure();
        }

        foreach (Dto.Discogs.DtoAlbum album in parseSearch.Where(x => x != null && x?.Id != 0))
        {
            OperationResult<string> masterRequest = await GetReleaseByIDJson(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (masterRequest.IsFailed)
            {
                continue;
            }

            bool isReleaseParsed = _endPointReleases.TryParse(masterRequest.Content, out Dto.Discogs.DtoMaster? parsedRelease);
            if (!isReleaseParsed || parsedRelease is null)
            {
                return result.WithFailure();
            }
            if (DateTime.TryParse(parsedRelease.Released, out DateTime parsedReleaseDate))
            {
                if (parsedReleaseDate.Month == DateTime.Now.Month)
                {
                    return result.WithValue(parsedRelease.MapToAlbum()).WithSuccess();
                }
            }
        }
        return result.WithFailure();

    }
  
    public async Task<HttpServiceResult<AlbumModel>> GetReleaseByName(string releaseTitle, CancellationToken cancellationToken)
    {
        HttpServiceResult<AlbumModel> result = new();

        if (string.IsNullOrEmpty(releaseTitle?.Trim()))
        {
            return result.WithFailure();
        }

        OperationResult<string> searchRequest = await this.GetSearchByReleaseTitleAndYearJson(releaseTitle, DateTime.Now.Year, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            result.Affect(searchRequest);
            return result;
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<Dto.Discogs.DtoAlbum>? parseSearch);
        if (!isSearchParsed)
        {
            return result.WithFailure();
        }

        if (parseSearch == null || parseSearch.Any(x => x != null && x?.Id != 0) == false)
        {
            return result.WithFailure();
        }

        foreach (Dto.Discogs.DtoAlbum album in parseSearch.Where(x => x != null && x?.Id != 0))
        {
            OperationResult<string> masterRequest = await GetReleaseByIDJson(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (masterRequest.IsFailed)
            {
                continue;
            }

            bool isReleaseParsed = _endPointReleases.TryParse(masterRequest.Content, out Dto.Discogs.DtoMaster? parsedRelease);
            if (!isReleaseParsed || parsedRelease is null)
            {
                return result.WithFailure();
            }
            if (DateTime.TryParse(parsedRelease.Released, out DateTime parsedReleaseDate))
            {
                if (parsedReleaseDate.Month == DateTime.Now.Month)
                {
                    return result.WithValue(parsedRelease.MapToAlbum()).WithSuccess();
                }
            }
        }

        return result.WithFailure();

    }
    #endregion

    #region Get Methods
    private async Task<OperationResult<string>> GetSearchByReleaseTitleAndYearJson(string nameAlbum, int year, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"release_title={nameAlbum}&year={year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }
    private async Task<OperationResult<string>> GetSearchByArtistNameAndYearJson(string nameArtist, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"artist={nameArtist}&year={DateTime.UtcNow.Year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }
    private async Task<OperationResult<string>> GetReleaseByIDJson(string id, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointReleases.Get(id), cancellationToken).ConfigureAwait(false);
    }
    #endregion
}

