namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for endpoint queries that retrieve collections of items with optional query filtering.
/// </summary>
public interface IEndpointListQuery : IEndpointQuery
{
    /// <summary>
    /// Gets the endpoint URL for retrieving a collection of items with optional query parameters.
    /// </summary>
    public string Get(string query = "");
}
