using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Extensions;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
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

    public async Task<OperationResultList<DtoDiscogsRelease>> SearchAsync(string releaseTitle, CancellationToken cancellationToken)
    {
        OperationResultList<DtoDiscogsRelease> result = new();

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

        List<DtoDiscogsAlbum> releasesFound = releasesResult.Content.Where(x => x != null && x?.Id != 0).ToList();
        VinylStrategyLogs.DiscogsReleaseSearchTitleResultsCount(_logger, releasesFound.Count, releaseTitle);

        foreach (DtoDiscogsAlbum album in releasesFound)
        {
            OperationResult<DtoDiscogsRelease> releaseRequest = await _discogService.GetReleaseByID(album.Id.ToString(CultureInfo.CurrentCulture), cancellationToken).ConfigureAwait(false);

            if (releaseRequest.IsFailed)
            {
                VinylStrategyLogs.FailedDiscogsGetReleaseById(_logger, album.Id, releaseRequest.ErrorCode, releaseRequest.ErrorMessage);
                continue;
            }
      
            bool isReleaseDateParsed = DateTime.TryParse(releaseRequest.Content.Released, out DateTime parsedReleaseDate);
            if (isReleaseDateParsed && parsedReleaseDate >= DateTime.Now.AddDays(-50))
            {
                result.Content.Add(releaseRequest.Content); 
                VinylStrategyLogs.FoundDiscogsReleaseByTitle(_logger, releaseRequest.Content.Title ?? "", releaseTitle);
            }
        }

        if (result.HasContent)
        {
            return result.WithSuccess();
        }
        else
        {
            VinylStrategyLogs.FailedDiscogsStrategyBytitle(_logger, releaseTitle);
            return result.WithError($"Search by release title failed, no results found this month for: '{releaseTitle}'");
        }
    }
}
