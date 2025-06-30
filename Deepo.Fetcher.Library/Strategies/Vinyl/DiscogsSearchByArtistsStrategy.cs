using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Globalization;

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

    public async Task<OperationResult<DtoDiscogsRelease>> SearchAsync(string artistName, CancellationToken cancellationToken)
    {
        OperationResult<DtoDiscogsRelease> result = new();

        if (string.IsNullOrEmpty(artistName?.Trim()))
        {
            VinylStrategyLogs.ArgumentNull(_logger, nameof(artistName));
            return result.WithError($"Empty required argument: {nameof(artistName)}");
        }

        OperationResult<IEnumerable<DtoDiscogsAlbum>> releasesResult = await _discogRepository.GetSearchByArtistNameAndYear(artistName, cancellationToken).ConfigureAwait(false);

        if (releasesResult.IsFailed)
        {
            VinylStrategyLogs.FailedDiscogsSearchByArtist(_logger, artistName, releasesResult.ErrorCode, releasesResult.ErrorMessage);
            return result.WithError("Discogs search by artist failed");
        }

        foreach (DtoDiscogsAlbum album in releasesResult.Content.Where(x => x != null && x?.Id != 0))
        {
            OperationResult<DtoDiscogsRelease> releaseRequest = await _discogRepository.GetReleaseByID(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (releaseRequest.IsFailed)
            {
                VinylStrategyLogs.FailedDiscogsGetReleaseById(_logger, album.Id, releaseRequest.ErrorCode, releaseRequest.ErrorMessage);
                continue;
            }

            bool isReleaseDateParsed = DateTime.TryParse(releaseRequest.Content.Released, out DateTime parsedReleaseDate);
            if (isReleaseDateParsed && parsedReleaseDate.Month == DateTime.Now.Month)
            {
                VinylStrategyLogs.FoundDiscogsReleaseByArtist(_logger, releaseRequest.Content.Title ?? "", artistName);
                return result.WithSuccess().WithValue(releaseRequest.Content);
            }
        }
        VinylStrategyLogs.FailedDiscogsStrategyByArtist(_logger, artistName);
        return result.WithError($"Search by artists failed, no results found for: '{artistName}'");
    }
}
