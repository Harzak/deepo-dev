using Deepo.Fetcher.Library.Fetcher.Vinyl;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Factory interface for creating vinyl fetch pipeline instances.
/// Provides a centralized way to create and configure vinyl fetch pipelines.
/// </summary>
public interface IVinylFetchPipelineFactory
{
    /// <summary>
    /// Creates a new instance of a vinyl fetch pipeline.
    /// </summary>
    /// <returns>A configured vinyl fetch pipeline instance.</returns>
    VinylFetchPipeline Create();
}
