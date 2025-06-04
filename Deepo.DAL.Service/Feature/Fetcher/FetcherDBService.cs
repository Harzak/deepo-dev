using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Service.Feature.Fetcher;

public class FetcherDBService : IFetcherDBService
{
    private readonly Models.DEEPOContext _dbContext;
    public FetcherDBService(Models.DEEPOContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ReadOnlyCollection<Models.Fetcher>? GetAll()
    {
        return _dbContext.Fetchers.ToList().AsReadOnly();
    }

    public ReadOnlyCollection<Models.V_FetcherExtended>? GetAllExtended()
    {
        return _dbContext.V_FetcherExtendeds.ToList().AsReadOnly();
    }

    public Models.Fetcher? GetByName(string name)
    {
        return _dbContext.Fetchers.FirstOrDefault(x => x.Name == name);
    }

    public Models.V_FetcherExtended? GetExtended(string id)
    {
        return _dbContext.V_FetcherExtendeds.Where(x => x.Fetcher_GUID == id).FirstOrDefault();
    }
}
