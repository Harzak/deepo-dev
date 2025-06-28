using Deepo.DAL.EF.Models;
using Deepo.DAL.Repository.Interfaces;

namespace Deepo.DAL.Repository.Feature.Fetcher;

public class FetcherHttpRequestRepository : IFetcherHttpRequestRepository
{
    private readonly DEEPOContext _dbContext;

    public FetcherHttpRequestRepository(DEEPOContext dbContext)
    {
        _dbContext = dbContext;
    }

    public HttpRequest? GetLast()
    {
        return _dbContext.HttpRequests.OrderByDescending(x => x.HttpRequest_ID).FirstOrDefault();
    }
}