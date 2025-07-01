using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Extensions;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Web;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

public class DiscogsSearchByReleaseTitleStrategy
{
    private readonly ILogger _logger;
    private readonly IDiscogRepository _discogService;

    public DiscogsSearchByReleaseTitleStrategy(IDiscogRepository discogService, ILogger logger)
    {
        _discogService = discogService;
        _logger = logger;
    }

    public async Task<OperationResult<DtoDiscogsRelease>> SearchAsync(string releaseTitle, CancellationToken cancellationToken)
    {
        OperationResult<DtoDiscogsRelease> result = new();

        if (string.IsNullOrEmpty(releaseTitle?.Trim()))
        {
            VinylStrategyLogs.ArgumentNull(_logger, nameof(releaseTitle));
            return result.WithError($"Empty required argument: {releaseTitle}");
        }

        string sanitizedTitle = HttpUtility.UrlEncode(releaseTitle.Trim());
        OperationResult<IEnumerable<DtoDiscogsAlbum>> releasesResult = await _discogService.GetSearchByReleaseTitleAndYear(sanitizedTitle, DateTime.Now.Year, cancellationToken).ConfigureAwait(false);

        if (releasesResult.IsFailed)
        {
            VinylStrategyLogs.FailedDiscogsSearchByTitle(_logger, releaseTitle, releasesResult.ErrorCode, releasesResult.ErrorMessage);
            return result.WithError("Discogs search by release title failed");
        }

        foreach (DtoDiscogsAlbum album in releasesResult.Content.Where(x => x != null && x?.Id != 0))
        {
            OperationResult<DtoDiscogsRelease> releaseRequest = await _discogService.GetReleaseByID(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);
            if (releaseRequest.IsFailed)
            {
                VinylStrategyLogs.FailedDiscogsGetReleaseById(_logger, album.Id, releaseRequest.ErrorCode, releaseRequest.ErrorMessage);
                continue;
            }

            bool isReleaseDateParsed = DateTime.TryParse(releaseRequest.Content.Released, out DateTime parsedReleaseDate);
            if (isReleaseDateParsed && parsedReleaseDate.IsSameMonthAndYear(DateTime.Now))
            {
                VinylStrategyLogs.FoundDiscogsReleaseByTitle(_logger, releaseRequest.Content.Title ?? "", releaseTitle);
                return result.WithSuccess().WithValue(releaseRequest.Content);
            }
        }
        VinylStrategyLogs.FailedDiscogsStrategyBytitle(_logger, releaseTitle);
        return result.WithError($"Search by release title failed, no results found for: '{releaseTitle}'");
    }
}
