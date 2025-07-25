﻿using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Repositories.Discogs.Endpoint;
using Deepo.Framework.Results;
using Deepo.Framework.Time;
using Deepo.Framework.Web.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Deepo.Fetcher.Library.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Deepo.Fetcher.Library.Repositories.Discogs;

/// <summary>
/// Provides access to Discogs API data.
/// Handles searching for albums and retrieving release information from the Discogs database.
/// </summary>
internal sealed class DiscogRepository : HttpService, IDiscogRepository
{
    private readonly HttpServiceOption _options;
    private readonly EndPointSearch _endPointSearch;
    private readonly EndPointReleases _endPointReleases;

    public DiscogRepository(IHttpClientFactory httpClientFactory,
        IOptions<HttpServicesOption> options,
        ILogger<DiscogRepository> logger)
    : base(httpClientFactory, 
        new DateTimeFacade(), 
        options.Value.Discogs ?? throw new ArgumentNullException("options.Value.SpotifyAuth"), 
        logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Discogs, nameof(options.Value.Discogs));

        _options = options.Value.Discogs;
        _endPointSearch = new EndPointSearch(_options, logger);
        _endPointReleases = new EndPointReleases(_options, logger);

        base.SetAuthorization("Discogs", $"token={_options.Token}");
    }

    /// <summary>
    /// Searches for albums by release title and year from the Discogs database.
    /// Filters results to vinyl format releases only.
    /// </summary>
    /// <param name="releaseTitle">The title of the release to search for.</param>
    /// <param name="year">The year of the release to search for.</param>
    /// <returns>An operation result containing a collection of matching Discogs albums.</returns>
    public async Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByReleaseTitleAndYear(string releaseTitle, int year, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = new();

        OperationResult<string> searchRequest = await this.GetSearchByReleaseTitleAndYearJson(releaseTitle, DateTime.Now.Year, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            return result.Affect(searchRequest);
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<DtoDiscogsAlbum>? parseSearch);
        if (isSearchParsed && parseSearch != null)
        {
            return result.WithSuccess().WithValue(parseSearch);
        }

        return result.WithError("Failed to parse Discogs search data");
    }

    private async Task<OperationResult<string>> GetSearchByReleaseTitleAndYearJson(string releaseTitle, int year, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"release_title={releaseTitle}&year={year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Searches for albums by artist name from the Discogs database.
    /// Filters results to vinyl format releases from the current year.
    /// </summary>
    /// <param name="nameArtist">The name of the artist to search for.</param>
    /// <returns>An operation result containing a collection of matching Discogs albums.</returns>
    public async Task<OperationResult<IEnumerable<DtoDiscogsAlbum>>> GetSearchByArtistNameAndYear(string nameArtist, CancellationToken cancellationToken)
    {
        OperationResult<IEnumerable<DtoDiscogsAlbum>> result = new();

        OperationResult<string> searchRequest = await GetSearchByArtistNameAndYearJson(nameArtist, cancellationToken).ConfigureAwait(false);
        if (searchRequest.IsFailed)
        {
            return result.Affect(searchRequest);
        }

        bool isSearchParsed = _endPointSearch.TryParse(searchRequest.Content, out IEnumerable<DtoDiscogsAlbum>? parseSearch);
        if (isSearchParsed && parseSearch != null)
        {
            return result.WithSuccess().WithValue(parseSearch);
        }

        return result.WithError("Failed to parse Discogs search data");
    }
    
    private async Task<OperationResult<string>> GetSearchByArtistNameAndYearJson(string nameArtist, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointSearch.Get($"artist={nameArtist}&year={DateTime.UtcNow.Year}&format=vinyl&type=release"), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a specific release by its ID from the Discogs database.
    /// </summary>
    /// <param name="id">The unique¨Discogs identifier of the release.</param>
    /// <returns>An operation result containing the Discogs release details.</returns>
    public async Task<OperationResult<DtoDiscogsRelease>> GetReleaseByID(string id, CancellationToken cancellationToken)
    {
        OperationResult<DtoDiscogsRelease> result = new();

        OperationResult<string> masterRequest = await GetReleaseByIDJson(id, cancellationToken).ConfigureAwait(false);
        if (masterRequest.IsFailed)
        {
            return result.Affect(masterRequest);
        }

        bool isReleaseParsed = _endPointReleases.TryParse(masterRequest.Content, out DtoDiscogsRelease? parsedRelease);
        if (isReleaseParsed && parsedRelease != null)
        {
            return result.WithSuccess().WithValue(parsedRelease);
        }

        return result.WithError("Failed to parse Discogs release return data");
    }
    
    private async Task<OperationResult<string>> GetReleaseByIDJson(string id, CancellationToken cancellationToken)
    {
        return await base.GetAsync(_endPointReleases.Get(id), cancellationToken).ConfigureAwait(false);
    }
}

