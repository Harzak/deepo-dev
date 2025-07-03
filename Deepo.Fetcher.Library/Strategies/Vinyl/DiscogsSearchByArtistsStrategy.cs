using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Extensions;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Web;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

internal class DiscogsSearchByArtistsStrategy
{
    private readonly ILogger _logger;
    private readonly IDiscogRepository _discogRepository;

    public DiscogsSearchByArtistsStrategy(IDiscogRepository discogRepository, ILogger logger)
    {
        _discogRepository = discogRepository;
        _logger = logger;
    }

    public async Task<OperationResultList<DtoDiscogsRelease>> SearchAsync(string artistName, CancellationToken cancellationToken)
    {
        OperationResultList<DtoDiscogsRelease> result = new();

        if (string.IsNullOrEmpty(artistName?.Trim()))
        {
            VinylStrategyLogs.ArgumentNull(_logger, nameof(artistName));
            return result.WithError($"Empty required argument: {nameof(artistName)}");
        }

        string sanitizedName = HttpUtility.UrlEncode(artistName.Trim());
        OperationResult<IEnumerable<DtoDiscogsAlbum>> releasesResult = await _discogRepository.GetSearchByArtistNameAndYear(sanitizedName, cancellationToken).ConfigureAwait(false);

        if (releasesResult.IsFailed)
        {
            VinylStrategyLogs.FailedDiscogsSearchByArtist(_logger, artistName, releasesResult.ErrorCode, releasesResult.ErrorMessage);
            return result.WithError("Discogs search by artist failed");
        }

        List<DtoDiscogsAlbum> releasesFound = releasesResult.Content.Where(x => x != null && x?.Id != 0).ToList();
        VinylStrategyLogs.DiscogsReleaseSearchArtistResultsCount(_logger, releasesFound.Count, artistName);

        foreach (DtoDiscogsAlbum album in releasesFound)
        {
            OperationResult<DtoDiscogsRelease> releaseRequest = await _discogRepository.GetReleaseByID(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (releaseRequest.IsFailed)
            {
                VinylStrategyLogs.FailedDiscogsGetReleaseById(_logger, album.Id, releaseRequest.ErrorCode, releaseRequest.ErrorMessage);
                continue;
            }

            bool isReleaseDateParsed = DateTime.TryParse(releaseRequest.Content.Released, out DateTime parsedReleaseDate);
            if (isReleaseDateParsed && parsedReleaseDate >= DateTime.Now.AddDays(-50))
            {
                result.Content.Add(releaseRequest.Content);
                VinylStrategyLogs.FoundDiscogsReleaseByArtist(_logger, releaseRequest.Content.Title ?? "", artistName);
            }
        }

        if (result.HasContent)
        {
            return result.WithSuccess();
        }
        else
        {
            VinylStrategyLogs.FailedDiscogsStrategyByArtist(_logger, artistName);
            return result.WithError($"Search by artists failed, no results found this month for: '{artistName}'");
        }
    }
}
