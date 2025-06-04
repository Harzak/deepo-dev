using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Service.Discogs;
using Deepo.Fetcher.Library.Service.Spotify;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Library.Fetcher.Fetch;

internal class FetchFactory : IFetchFactory
{
    private readonly IDiscogService _discogService;
    private readonly ISpotifyService _spotifyService;
    private readonly IReleaseAlbumDBService _releaseAlbumDBService;
    private readonly ILogger _logger;

    public FetchFactory(IDiscogService discogService,
        ISpotifyService spotifyService,
        IReleaseAlbumDBService releaseAlbumDBService,
        ILogger<FetchFactory> logger)
    {
        _discogService = discogService;
        _spotifyService = spotifyService;
        _releaseAlbumDBService = releaseAlbumDBService;
        _logger = logger;
    }

    public IFetch CreateFetchVinyl(Album initialData)
    {
        return new FetchVinyl(initialData, _discogService, _spotifyService, _releaseAlbumDBService, _logger);
    }
}

