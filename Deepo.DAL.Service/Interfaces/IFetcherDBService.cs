using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Service.Feature.Fetcher;

public interface IFetcherDBService
{
    ReadOnlyCollection<Models.Fetcher>? GetAll();
    ReadOnlyCollection<Models.V_FetcherExtended>? GetAllExtended();
    Models.V_FetcherExtended? GetExtended(string id);
    Models.Fetcher? GetByName(string name);
}
