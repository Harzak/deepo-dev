namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for basic endpoint query operations providing HTTP method support.
/// </summary>
public interface IEndpointQuery
{
    /// <summary>
    /// Gets the endpoint URL for HTTP OPTIONS requests to discover available methods and capabilities.
    /// </summary>
    public string Options();
    
    /// <summary>
    /// Gets the endpoint URL for HTTP TRACE requests to perform diagnostic operations.
    /// </summary>
    public string Trace();
}

