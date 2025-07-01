using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Fetcher.Vinyl;

public class VinylFetchPipelineFactory : IVinylFetchPipelineFactory
{
    private readonly ILogger<VinylFetchPipeline> _logger;
    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IVinylStrategiesFactory _strategiesFactory;
    private readonly IReleaseHistoryRepository _historyRepository;

    public VinylFetchPipelineFactory(IReleaseAlbumRepository releaseAlbumRepository,
        IVinylStrategiesFactory strategiesFactory,
        IReleaseHistoryRepository historyRepository,
        ILogger<VinylFetchPipeline> logger)
    {
        _releaseAlbumRepository = releaseAlbumRepository;
        _strategiesFactory = strategiesFactory;
        _historyRepository = historyRepository;
        _logger = logger;
    }

    public VinylFetchPipeline Create()
    {
        return new VinylFetchPipeline(_strategiesFactory, _releaseAlbumRepository, _historyRepository, _logger);
    }
}