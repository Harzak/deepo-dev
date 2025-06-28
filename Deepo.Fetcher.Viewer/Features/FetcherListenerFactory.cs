using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Viewer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Viewer.Features;

internal sealed class FetcherListenerFactory : IFetcherListenerFactory
{
    private readonly ILogger _logger;


    private readonly IReleaseAlbumDBService _releaseAlbumDBService;
    private readonly IFetcherHttpRequestDBService _fetcherHttpRequestDBService;
    private readonly DEEPOContext _db;
    private readonly string _connstring;

    public FetcherListenerFactory(DEEPOContext db,
        IReleaseAlbumDBService releaseAlbumDBService,
        IFetcherHttpRequestDBService fetcherHttpRequestDBService,
        ILogger<FetcherListenerFactory> logger)
    {
        _db = db;
        _releaseAlbumDBService = releaseAlbumDBService;
        _fetcherHttpRequestDBService = fetcherHttpRequestDBService;
        _connstring = _db.Database.GetDbConnection().ConnectionString;
        _logger = logger;
    }

    public IFetcherListener CreateFetcherListener()
    {
        return new FetcherListener(_connstring, _releaseAlbumDBService, _fetcherHttpRequestDBService, _logger);
    }
}