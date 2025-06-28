using Deepo.DAL.EF.Models;

namespace Deepo.DAL.Service.Interfaces;

public interface IFetcherHttpRequestDBService
{
    HttpRequest? GetLast();
}
