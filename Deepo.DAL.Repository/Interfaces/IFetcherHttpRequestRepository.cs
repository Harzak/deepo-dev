using Deepo.DAL.EF.Models;

namespace Deepo.DAL.Repository.Interfaces;

public interface IFetcherHttpRequestRepository
{
    HttpRequest? GetLast();
}
