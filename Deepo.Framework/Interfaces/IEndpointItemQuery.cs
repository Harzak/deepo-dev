namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for endpoint queries that operate on individual items with CRUD operations.
/// </summary>
public interface IEndpointItemQuery : IEndpointQuery
{
    /// <summary>
    /// Gets the endpoint URL for retrieving a specific item by its identifier.
    /// </summary>
    public string Get(string id);
    
    /// <summary>
    /// Gets the endpoint URL for creating a new item with the specified identifier and content.
    /// </summary>
    public string Post(string id, string content);
    
    /// <summary>
    /// Gets the endpoint URL for updating an item with the specified identifier and content.
    /// </summary>
    public string Put(string id, string content);
    
    /// <summary>
    /// Gets the endpoint URL for partially updating an item with the specified identifier and content.
    /// </summary>
    public string Patch(string id, string content);
    
    /// <summary>
    /// Gets the endpoint URL for deleting an item with the specified identifier.
    /// </summary>
    public string Delete(string id);
}

