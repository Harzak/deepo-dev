using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.LogMessage;

internal static partial class VinylStrategyLogs
{

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Argument null: {argumentName}")]
    public static partial void ArgumentNull(ILogger logger, string argumentName);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Discogs search failed for artist: {artist} | code: {code} | error: {error}")]
    public static partial void FailedDiscogsSearchByArtist(ILogger logger, string artist, string code, string error);

    [LoggerMessage(EventId = 2, Level = LogLevel.Error, Message = "Discogs get release failed for release_ID: {id} | code: {code} | error: {error}")]
    public static partial void FailedDiscogsGetReleaseById(ILogger logger, int id, string code, string error);

    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Found release: '{releaseName}' for artist: '{artistName}' in Discogs")]
    public static partial void FoundDiscogsReleaseByArtist(ILogger logger, string releaseName, string artistName);

    [LoggerMessage(EventId = 4, Level = LogLevel.Warning, Message = "Discogs search by artist failed, no result found for: {artistName}")]
    public static partial void FailedDiscogsStrategyByArtist(ILogger logger, string artistName);

    [LoggerMessage(EventId = 5, Level = LogLevel.Error, Message = "Discogs search failed for release title: {title} | code: {code} | error: {error}")]
    public static partial void FailedDiscogsSearchByTitle(ILogger logger, string title, string code, string error);

    [LoggerMessage(EventId = 6, Level = LogLevel.Information, Message = "Found release: '{releaseName}' for release title: '{releaseTitle}' in Discogs")]
    public static partial void FoundDiscogsReleaseByTitle(ILogger logger, string releaseName, string releaseTitle);

    [LoggerMessage(EventId = 7, Level = LogLevel.Warning, Message = "Discogs search by title failed, no result found for: {releaseTitle}")]
    public static partial void FailedDiscogsStrategyBytitle(ILogger logger, string releaseTitle);

    [LoggerMessage(EventId = 8, Level = LogLevel.Information, Message = "Discover a release on Spotify | artists: {artists} | title: {title}")]
    public static partial void SuccessSpotifyReleaseDiscovery(ILogger logger, string artists, string title);

    [LoggerMessage(EventId = 9, Level = LogLevel.Error, Message = "Spotify release discovery failed | code: {code} | error: {error} ")]
    public static partial void FailedSpotifyReleaseDiscovery(ILogger logger, string code, string error);

    [LoggerMessage(EventId = 10, Level = LogLevel.Error, Message = "All strategies failed for release: '{title}'")]
    public static partial void AllStrategiesFailed(ILogger logger, string title);
}
