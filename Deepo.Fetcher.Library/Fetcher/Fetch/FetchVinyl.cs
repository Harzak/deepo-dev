using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Configuration;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Service;
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
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;

    private readonly Dto.Spotify.Album _initialData;

    private HttpServiceResult<AlbumModel> _resultDiscogs;

    public FetchVinyl(Dto.Spotify.Album newRelease,
        IDiscogService discogService,
        ISpotifyService spotifyService,
        IReleaseAlbumRepository releaseAlbumRepository,
        ILogger logger)
    {
        _logger = logger;
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumRepository = releaseAlbumRepository;
        _initialData = newRelease;
        _resultDiscogs = null!;
    }

    protected override bool CanStart()
    {
        if (DateTime.TryParse(_initialData.ReleaseDate, out DateTime releaseDate))
        {
            return base.CanStart() && _initialData.Artists?.FirstOrDefault()?.Name?.IsNullOrEmpty() != true && releaseDate >= MinReleaseDate;
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
            string? releaseTitle = _initialData.Name?.Trim();
            if (!releaseTitle.IsNullOrEmpty())
            {
                _resultDiscogs = await _discogService.GetReleaseByName(releaseTitle, cancellationToken).ConfigureAwait(false);
                if (_resultDiscogs.IsFailed)
                {
                    string? artistName = _initialData.Artists?.FirstOrDefault()?.Name?.Trim();
                    if (!artistName.IsNullOrEmpty())
                    {
                        _resultDiscogs = await _discogService.GetReleaseByArtist(artistName, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
                return _resultDiscogs;
        })
        .ConfigureAwait(false);

        await EndWith(async () =>
        {
            return await _releaseAlbumRepository.Insert(_resultDiscogs.Content, cancellationToken).ConfigureAwait(false);
        })
        .ConfigureAwait(false);

    }

    protected override void Dispose(bool disposing)
    {
        _resultDiscogs = null!;
        base.Dispose(disposing);
    }
}
