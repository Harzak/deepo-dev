using Deepo.DAL.Service.Feature.Release;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Service;
using Deepo.Fetcher.Library.Service.Discogs;
using Deepo.Fetcher.Library.Service.Spotify;
using Framework.Common.Utils.Extension;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Deepo.Fetcher.Library.Tests")]

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal class FetchVinyl : FetchBase
{
    private readonly ILogger _logger;

    private readonly IDiscogService _discogService;
    private readonly ISpotifyService _spotifyService;
    private readonly IReleaseAlbumDBService _releaseAlbumDBService;

    private readonly Dto.Spotify.Album _initialData;
    private readonly string _artistName;

    private HttpServiceResult<IAuthor> _resultDiscogs1;
    private HttpServiceResult<AlbumModel> _resultDiscogs2;

    public FetchVinyl(Dto.Spotify.Album newRelease,
        IDiscogService discogService,
        ISpotifyService spotifyService,
        IReleaseAlbumDBService releaseAlbumDBService,
        ILogger logger)
    {
        _logger = logger;
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumDBService = releaseAlbumDBService;
        _initialData = newRelease;
        _artistName = _initialData.Artists?.First().Name ?? string.Empty;
        _resultDiscogs1 = null!;
        _resultDiscogs2 = null!;
    }

    protected override bool CanStart()
    {

        if (DateTime.TryParse(_initialData.ReleaseDate, out DateTime releaseDate))
        {
            return base.CanStart() && !_artistName.IsNullOrEmpty() && releaseDate >= MinReleaseDate;
        }
        return false;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        OnSucces(() =>
        {
            FetcherLogs.Success(_logger, nameof(FetchVinyl));
        });

        await StartWithAsync(async () =>
        {
            _resultDiscogs1 = await _discogService.GetArtist(_artistName, cancellationToken).ConfigureAwait(false);
            return _resultDiscogs1;
        })
        .ConfigureAwait(false);

        await ContinueWithAsync(async () =>
        {
            _resultDiscogs2 = await _discogService.GetLastReleaseByArtistID(_resultDiscogs1.Content.Provider_Identifier, cancellationToken).ConfigureAwait(false);
            return _resultDiscogs2;
        })
       .ConfigureAwait(false);

        await EndWith(async () =>
        {
            return await _releaseAlbumDBService.Insert(_resultDiscogs2.Content, cancellationToken).ConfigureAwait(false);
        })
        .ConfigureAwait(false);

    }

    protected override void Dispose(bool disposing)
    {
        _resultDiscogs1 = null!;
        _resultDiscogs2 = null!;
        base.Dispose(disposing);
    }
}
