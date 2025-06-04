using Deepo.DAL.Service.Feature.ReleaseAlbum;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Mappers;
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
    private readonly EndPointMasters _endPointMasters;
    private readonly EndPointArtistReleases _endPointArtistReleases;

    public DiscogService(IHttpClientFactory httpClientFactory, IOptions<HttpServicesOption> options, ILogger<DiscogService> logger)
    : base(httpClientFactory, new TimeProvider(), options.Value.Discogs, logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Discogs, nameof(options.Value.Discogs));

        _options = options.Value.Discogs;
        _endPointSearch = new EndPointSearch(_options, logger);
        _endPointMasters = new EndPointMasters(_options, logger);
        _endPointArtistReleases = new EndPointArtistReleases(_options, logger);

        base.SetAuthorization("Discogs", $"token={_options.Token}");
    }

    #region Public 
    public async Task<HttpServiceResult<IAuthor>> GetArtist(string nameArtist, CancellationToken cancellationToken)
    {
        HttpServiceResult<IAuthor> result = new();

        if (string.IsNullOrEmpty(nameArtist?.Trim()))
        {
            return result.WithError("Invalid id");
        }

        OperationResult<string> searchRequest = await GetSearchMasterByArtists(nameArtist, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            result.Affect(searchRequest);
            return result;
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out Dto.Discogs.Album? parsedAlbum) && parsedAlbum != null;
        if (!isSearchParsed || parsedAlbum is null)
        {
            return result.WithFailure();
        }

        OperationResult<string> masterRequest = await GetMasterByID(parsedAlbum.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
        if (masterRequest.IsFailed)
        {
            return result.WithFailure();
        }

        bool isMasterParsed = _endPointMasters.TryParse(masterRequest.Content, out Dto.Discogs.Master? parsedAlbum2);
        if (!isMasterParsed || parsedAlbum2 is null)
        {
            return result.WithFailure();
        }

        Dto.Discogs.Artist? artist = parsedAlbum2.Artists?.FirstOrDefault();
        if (artist == null)
        {
            return result.WithFailure();
        }

        return result.WithValue(artist).WithSuccess();
    }
    public async Task<HttpServiceResult<AlbumModel>> GetLastReleaseByArtistID(string id, CancellationToken cancellationToken)
    {
        HttpServiceResult<AlbumModel> result = new();

        if (string.IsNullOrEmpty(id?.Trim()))
        {
            return result.WithFailure();
        }

        OperationResult<string> lastReleaseRequest = await GetLastReleaseByArtistIDJson(id, cancellationToken).ConfigureAwait(false);
        if (lastReleaseRequest.IsFailed)
        {
            result.Affect(lastReleaseRequest);
            return result;
        }

        bool areAlbumsParsed = _endPointArtistReleases.TryParse(lastReleaseRequest.Content, out IEnumerable<Dto.Discogs.Release>? parsedAlbums);
        if (!areAlbumsParsed || parsedAlbums is null || !parsedAlbums.Any())
        {
            return result.WithFailure();
        }

        OperationResult<string> masterRequest = await GetMasterByID(parsedAlbums.First().Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
        if (masterRequest.IsFailed)
        {
            result.Affect(masterRequest);
            return result;
        }

        bool isMasterParsed = _endPointMasters.TryParse(masterRequest.Content, out Dto.Discogs.Master? parsedAlbum2);
        if (!isMasterParsed || parsedAlbum2 is null)
        {
            return result.WithFailure();
        }

        return result.WithValue(parsedAlbum2.MapToAlbum()).WithSuccess();
    }
    #endregion

    #region Get Methods
    private async Task<OperationResult<string>> GetReleaseJson(string nameAlbum, string nameArtist, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"release_title={nameAlbum}&artist={nameArtist}"), cancellationToken).ConfigureAwait(false);
    }
    private async Task<OperationResult<string>> GetSearchMasterByArtists(string nameArtist, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"artist={nameArtist}&year={DateTime.UtcNow.Year}"), cancellationToken).ConfigureAwait(false);
    }
    private async Task<OperationResult<string>> GetMasterByID(string id, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointMasters.Get(id), cancellationToken).ConfigureAwait(false);
    }
    private async Task<OperationResult<string>> GetLastReleaseByArtistIDJson(string id, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointArtistReleases.Get($"{id}/releases?sort=year&sort_order=desc"), cancellationToken).ConfigureAwait(false);
    }
    #endregion
}

