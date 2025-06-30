using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal class FetchFactory : IFetchFactory
{
    private readonly IDiscogRepository _discogService;
    private readonly ISpotifyRepository _spotifyService;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly ILogger _logger;

    public FetchFactory(IDiscogRepository discogService,
        ISpotifyRepository spotifyService,
        IReleaseAlbumRepository releaseAlbumRepository,
        ILogger<FetchFactory> logger)
    {
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumRepository = releaseAlbumRepository;
        _logger = logger;
    }

    public IFetch CreateFetchVinyl(DtoSpotifyAlbum initialData)
    {
        return new FetchVinyl(initialData, _discogService, _spotifyService, _releaseAlbumRepository, _logger);
    }
}

