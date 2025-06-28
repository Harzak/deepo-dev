using System.Collections.ObjectModel;
using Models = Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherRepository
{
    ReadOnlyCollection<Models.Fetcher>? GetAll();
    ReadOnlyCollection<Models.V_FetcherExtended>? GetAllExtended();
    Models.V_FetcherExtended? GetExtended(string id);
    Models.Fetcher? GetByName(string name);
}
