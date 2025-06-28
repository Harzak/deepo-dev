using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Service.Discogs;
using Deepo.Fetcher.Library.Service.Spotify;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal class FetchFactory : IFetchFactory
{
    private readonly IDiscogService _discogService;
    private readonly ISpotifyService _spotifyService;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly ILogger _logger;

    public FetchFactory(IDiscogService discogService,
        ISpotifyService spotifyService,
        IReleaseAlbumRepository releaseAlbumRepository,
        ILogger<FetchFactory> logger)
    {
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumRepository = releaseAlbumRepository;
        _logger = logger;
    }

    public IFetch CreateFetchVinyl(Album initialData)
    {
        return new FetchVinyl(initialData, _discogService, _spotifyService, _releaseAlbumRepository, _logger);
    }
}

