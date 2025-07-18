using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Provides a centralized way to instantiate different types of fetcher workers.
/// </summary>
internal interface IFetcherFactory
{
    /// <summary>
    /// Creates a fetcher worker instance based on the provided code.
    /// </summary>
    /// <param name="code">The code identifying the type of fetcher to create.</param>
    /// <returns>A worker instance corresponding to the specified code.</returns>
    IWorker CreateFetcher(string code);
}