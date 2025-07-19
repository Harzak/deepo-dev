namespace Deepo.Fetcher.Viewer.Interfaces;

/// <summary>
/// Defines the contract for creating fetcher listener instances.
/// </summary>
internal interface IFetcherListenerFactory
{
    /// <summary>
    /// Creates a new instance of a fetcher listener with configured dependencies.
    /// </summary>
    IFetcherListener CreateFetcherListener();
}