using Deepo.DAL.EF.Models;
using Deepo.DAL.Service.Interfaces;

namespace Deepo.DAL.Service.Feature.Fetcher;

public class FetcherHttpRequestDBService : IFetcherHttpRequestDBService
{
    private readonly DEEPOContext _dbContext;

    public FetcherHttpRequestDBService(DEEPOContext dbContext)
    {
        _dbContext = dbContext;
    }

    public HttpRequest? GetLast()
    {
        return _dbContext.HttpRequests.OrderByDescending(x => x.HttpRequest_ID).FirstOrDefault();
    }
}