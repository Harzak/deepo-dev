using Framework.Common.Worker.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

internal interface IFetcherFactory
{
    IWorker CreateFetcher(string code);
}