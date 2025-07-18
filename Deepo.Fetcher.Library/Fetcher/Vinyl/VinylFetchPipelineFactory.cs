using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Library.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Fetcher.Vinyl;

/// <summary>
/// Creates vinyl fetch pipeline instances.
/// Provides configured instances with all necessary dependencies for vinyl data processing.
/// </summary>
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

    /// <summary>
    /// Creates a new instance of a vinyl fetch pipeline with all necessary dependencies configured.
    /// </summary>
    /// <returns>A configured vinyl fetch pipeline instance ready for use.</returns>
    public VinylFetchPipeline Create()
    {
        return new VinylFetchPipeline(_strategiesFactory, _releaseAlbumRepository, _historyRepository, _logger);
    }
}