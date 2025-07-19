using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

/// <summary>
/// Provides an abstract base class for endpoint consumers that handle single-item CRUD operations with HTTP method support.
/// </summary>
public abstract class SingleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointItemQuery
{
    protected SingleResultEndpointConsumer(ILogger logger) : base(logger) { }

    /// <summary>
    /// Gets the endpoint URL for retrieving a specific item by its identifier.
    /// </summary>
    public abstract string Get(string id);

    /// <summary>
    /// Gets the endpoint URL for creating a new item with the specified identifier and content.
    /// </summary>
    public virtual string Post(string id, string content)
    {
        throw new NotSupportedException("POST method is not supported for this endpoint.");
    }

    /// <summary>
    /// Gets the endpoint URL for updating an item with the specified identifier and content.
    /// </summary>
    public  virtual string Put(string id, string content)
    {
        throw new NotSupportedException("PUT method is not supported for this endpoint.");
    }

    /// <summary>
    /// Gets the endpoint URL for partially updating an item with the specified identifier and content.
    /// </summary>
    public virtual string Patch(string id, string content)
    {
        throw new NotSupportedException("PATCH method is not supported for this endpoint.");
    }

    /// <summary>
    /// Gets the endpoint URL for deleting an item with the specified identifier.
    /// </summary>
    public virtual string Delete(string id)
    {
        throw new NotSupportedException("DELETE method is not supported for this endpoint.");
    }

    /// <summary>
    /// Gets the endpoint URL for HTTP OPTIONS requests to discover available methods and capabilities.
    /// </summary>
    public virtual string Options()
    {
        throw new NotSupportedException("OPTIONS method is not supported for this endpoint.");
    }

    /// <summary>
    /// Gets the endpoint URL for HTTP TRACE requests to perform diagnostic operations.
    /// </summary>
    public virtual string Trace()
    {
        throw new NotSupportedException("TRACE method is not supported for this endpoint.");
    }
}

