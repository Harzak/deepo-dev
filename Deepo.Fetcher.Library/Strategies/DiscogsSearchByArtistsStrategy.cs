using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.Result;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.Mappers;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Strategies;

internal class DiscogsSearchByArtistsStrategy
{
    private readonly ILogger _logger;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IDiscogRepository _discogService;

    public DiscogsSearchByArtistsStrategy(IDiscogRepository discogService, IReleaseAlbumRepository releaseAlbumRepository, ILogger logger)
    {
        _discogService = discogService;
        _releaseAlbumRepository = releaseAlbumRepository;
        _logger = logger;
    }

    public async Task<OperationResult> StartAsync(string artistName, CancellationToken cancellationToken)
    {
        OperationResult result = new();

        if (string.IsNullOrEmpty(artistName?.Trim()))
        {
            return result.WithFailure();
        }

        OperationResult<IEnumerable<DtoDiscogsAlbum>?> releasesResult = await _discogService.GetSearchByArtistNameAndYear(artistName, cancellationToken).ConfigureAwait(false);

        if (releasesResult.IsFailed)
        {
            return result.WithError(releasesResult.ErrorMessage);
        }

        if (releasesResult.Content == null)
        {
            return result.WithFailure();
        }

        bool isInserted = false;
        foreach (DtoDiscogsAlbum album in releasesResult.Content.Where(x => x != null && x?.Id != 0))
        {
            OperationResult<DtoDiscogsRelease?> masterRequest = await _discogService.GetReleaseByID(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (masterRequest.IsFailed)
            {
                continue;
            }
            if (masterRequest.Content == null)
            {
                continue;
            }
            bool isReleaseDateParsed = DateTime.TryParse(masterRequest.Content.Released, out DateTime parsedReleaseDate);

            if (parsedReleaseDate.Month != DateTime.Now.Month)
            {
                continue;
            }

            AlbumModel mappedModel = masterRequest.Content.MapToAlbum();

            DatabaseOperationResult insertResult = await _releaseAlbumRepository.Insert(mappedModel, cancellationToken).ConfigureAwait(false);
            if (insertResult.IsFailed)
            {
                continue;
            }
            isInserted = true;
        }

        result.IsSuccess = isInserted;
        return result;
    }
}
