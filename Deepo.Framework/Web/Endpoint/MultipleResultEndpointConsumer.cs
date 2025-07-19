using Deepo.Framework.Interfaces;
using Microsoft.Extensions.Logging;

namespace Deepo.Framework.Web.Endpoint;

/// <summary>
/// Provides an abstract base class for endpoint consumers that handle collection-based operations with HTTP method support.
/// </summary>
public abstract class MultipleResultEndpointConsumer<TModel> : EndpointConsumerBase<TModel>, IEndpointListQuery
{
    protected MultipleResultEndpointConsumer(ILogger logger) : base(logger) { }

    /// <summary>
    /// Gets the endpoint URL for retrieving a collection of items with optional query parameters.
    /// </summary>
    public abstract string Get(string query = "");

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
