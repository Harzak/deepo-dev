using Framework.Common.Worker.Interfaces;

namespace Deepo.Fetcher.Library.Fetcher;

internal interface IFetcherFactory
{
    IWorker CreateFetcher(string code);
}

