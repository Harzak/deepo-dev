using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Viewer.Features.Listener;

/// <summary>
/// Factory for creating fetcher listener instances with configured dependencies.
/// </summary>
internal sealed class FetcherListenerFactory : IFetcherListenerFactory
{
    private readonly ILogger _logger;


    private readonly IReleaseAlbumRepository _releaseAlbumRepository;
    private readonly IFetcherHttpRequestRepository _httpRequestRepository;
    private readonly IFetcherExecutionRepository _fetcherExecutionRepository;
    private readonly DEEPOContext _db;
    private readonly string _connstring;

    public FetcherListenerFactory(DEEPOContext db,
        IReleaseAlbumRepository releaseAlbumRepository,
        IFetcherHttpRequestRepository httpRequestRepository,
        IFetcherExecutionRepository fetcherExecutionRepository,
        ILogger<FetcherListenerFactory> logger)
    {
        _db = db;
        _releaseAlbumRepository = releaseAlbumRepository;
        _httpRequestRepository = httpRequestRepository;
        _fetcherExecutionRepository = fetcherExecutionRepository;
        _connstring = _db.Database.GetDbConnection().ConnectionString;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new instance of a fetcher listener configured with the necessary repositories and database connection.
    /// </summary>
    public IFetcherListener CreateFetcherListener()
    {
        return new FetcherListener(_connstring, _releaseAlbumRepository, _httpRequestRepository, _fetcherExecutionRepository, _logger);
    }
}