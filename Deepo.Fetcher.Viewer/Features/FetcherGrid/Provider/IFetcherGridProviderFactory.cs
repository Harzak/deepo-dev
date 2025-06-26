namespace Deepo.Fetcher.WPF.Features.FetcherGrid.Provider
{
    internal interface IFetcherGridProviderFactory
    {
        IFetcherGridProvider CreateFetcherGridProvider(string id);
    }
}
