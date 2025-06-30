using Deepo.DAL.Repository.Feature.Release;
using Deepo.DAL.Repository.Interfaces;
using Deepo.DAL.Repository.Result;
using Deepo.Fetcher.Library.Dto.Discogs;
using Deepo.Fetcher.Library.Dto.Spotify;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Fetcher.Library.LogMessage;
using Deepo.Fetcher.Library.Mappers;
using Deepo.Fetcher.Library.Utils;
using Framework.Common.Utils.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Strategies.Vinyl;

public class VinylStrategy : IVynilStrategy
{
    private readonly ILogger<VinylStrategy> _logger;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IVinylStrategiesFactory _strategiesFactory;


    private Action? _onSucces;
    private Action? _onFailure;

    public VinylStrategy(IReleaseAlbumRepository releaseAlbumRepository, IVinylStrategiesFactory strategiesFactory, ILogger<VinylStrategy> logger)
    {
        _releaseAlbumRepository = releaseAlbumRepository;

        _strategiesFactory = strategiesFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await foreach (DtoSpotifyAlbum spotifyAlbum in _strategiesFactory.DiscoverSpotifyFrenchMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            OperationResult<DtoDiscogsRelease> searchRelease = await _strategiesFactory.SearchDiscogsByTitleAsync(spotifyAlbum.Name!, cancellationToken).ConfigureAwait(false);
            if (searchRelease.IsSuccess)
            {
                if (await InsertReleaseAlbumAsync(searchRelease.Content, cancellationToken).ConfigureAwait(false))
                {
                    VinylStrategyLogs.SuccessStrategy(_logger, "Market FR by title", spotifyAlbum.Name!);
                }
                continue;
            }

            foreach (DtoSpotifyArtist artist in spotifyAlbum.Artists!)
            {
                searchRelease = await _strategiesFactory.SearchDiscogsByArtistAsync(artist.Name!, cancellationToken).ConfigureAwait(false);
                if (searchRelease.IsSuccess)
                {
                    if (await InsertReleaseAlbumAsync(searchRelease.Content, cancellationToken).ConfigureAwait(false))
                    {
                        VinylStrategyLogs.SuccessStrategy(_logger, "Market FR by artists", spotifyAlbum?.Name!);
                    }
                }
            }

            VinylStrategyLogs.AllStrategiesFailed(_logger, spotifyAlbum?.Name!);
        }

        await foreach (DtoSpotifyAlbum spotifyAlbum in _strategiesFactory.DiscoverSpotifyNorthAmericanMarketAsync(cancellationToken).ConfigureAwait(false))
        {
            OperationResult<DtoDiscogsRelease> searchRelease = await _strategiesFactory.SearchDiscogsByTitleAsync(spotifyAlbum.Name!, cancellationToken).ConfigureAwait(false);
            if (searchRelease.IsSuccess)
            {
                if (await InsertReleaseAlbumAsync(searchRelease.Content, cancellationToken).ConfigureAwait(false))
                {
                    VinylStrategyLogs.SuccessStrategy(_logger, "Market US by title", spotifyAlbum?.Name!);
                }
                continue;
            }

            searchRelease = await _strategiesFactory.SearchDiscogsByArtistAsync(spotifyAlbum?.Artists?.FirstOrDefault()?.Name ?? string.Empty, cancellationToken).ConfigureAwait(false);
            if (searchRelease.IsSuccess)
            {
                if (await InsertReleaseAlbumAsync(searchRelease.Content, cancellationToken).ConfigureAwait(false))
                {
                    VinylStrategyLogs.SuccessStrategy(_logger, "Market US by artists", spotifyAlbum?.Name!);
                }
                continue;
            }

            VinylStrategyLogs.AllStrategiesFailed(_logger, spotifyAlbum?.Name!);
        }
    }



    private async Task<bool> InsertReleaseAlbumAsync(DtoDiscogsRelease release, CancellationToken cancellationToken)
    {
        AlbumModel mappedModel = release.MapToAlbum();
        return (await _releaseAlbumRepository.InsertAsync(mappedModel, cancellationToken).ConfigureAwait(false)).IsSuccess;
    }

    public void OnSuccess(Action action)
    {
        _onSucces = action;
    }

    public void OnFailure(Action action)
    {
        _onFailure = action;
    }
}

