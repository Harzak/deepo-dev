using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Configuration;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Repositories;
using Deepo.Fetcher.Library.Strategies;
using Framework.Common.Utils.Extension;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Deepo.Fetcher.Library.Tests")]

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal class FetchVinyl : FetchBase
{
    private readonly ILogger _logger;

    private readonly IDiscogRepository _discogService;
    private readonly ISpotifyRepository _spotifyService;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;

    private readonly Dto.Spotify.DtoSpotifyAlbum _initialData;

    public FetchVinyl(Dto.Spotify.DtoSpotifyAlbum newRelease,
        IDiscogRepository discogService,
        ISpotifyRepository spotifyService,
        IReleaseAlbumRepository releaseAlbumRepository,
        ILogger logger)
    {
        _logger = logger;
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumRepository = releaseAlbumRepository;
        _initialData = newRelease;
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

        DiscogsSearchByReleaseTitleStrategy discogsSearchByReleaseTitleStrategy = new(_discogService, _releaseAlbumRepository, _logger);
        OperationResult result = await discogsSearchByReleaseTitleStrategy.StartAsync(_initialData?.Name, cancellationToken).ConfigureAwait(false);
        if (result.IsFailed)
        {
            DiscogsSearchByArtistsStrategy discogsSearchByArtistsStrategy = new(_discogService, _releaseAlbumRepository, _logger);
            result = await discogsSearchByArtistsStrategy.StartAsync(_initialData?.Artists?.FirstOrDefault()?.Name ?? string.Empty, cancellationToken).ConfigureAwait(false);
        }

    }
}
