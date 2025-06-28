using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;
using Deepo.Fetcher.Viewer.Features.FetcherGrid.Provider;
using Deepo.Fetcher.Viewer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deepo.Fetcher.Viewer.Features.FetcherHttpRequest.Provider;

internal sealed class FetcherHttpRequestProviderFactory : IFetcherHttpRequestProviderFactory
{
    private readonly ILogger _logger;

    private const string subscriptionQuery = "SELECT [HttpRequest_ID] FROM [fetcher].[HttpRequest]";

    private readonly IFetcherHttpRequestDBService _fetcherHttpRequestDBService;
    private readonly DEEPOContext _db;
    private readonly string _connstring;

    public FetcherHttpRequestProviderFactory(DEEPOContext db, IFetcherHttpRequestDBService fetcherHttpRequestDBService, ILogger<FetcherGridProviderFactory> logger)
    {
        _db = db;
        _fetcherHttpRequestDBService = fetcherHttpRequestDBService;
        _connstring = _db.Database.GetDbConnection().ConnectionString;
        _logger = logger;
    }

    public IFetcherHttpRequestProvider CreateFetcherHttpRequestProvider()
    {
        return new FetcherHttpRequestProvider(_connstring, subscriptionQuery, _fetcherHttpRequestDBService, _logger);
    }
}