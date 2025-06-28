namespace Deepo.Fetcher.Viewer.Interfaces;

internal interface IFetcherGridProviderFactory
{
    IFetcherGridProvider CreateFetcherGridProvider(string id);
}