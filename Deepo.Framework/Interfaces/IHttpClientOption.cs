using System;

namespace Deepo.Framework.Interfaces;

/// <summary>
/// Defines the contract for HTTP client configuration options including base address and identification properties.
/// </summary>
public interface IHttpClientOption
{
    /// <summary>
    /// Gets or sets the base address URI for HTTP requests.
    /// </summary>
    Uri BaseAddress { get; set; }
    
    /// <summary>
    /// Gets or sets the name identifier for the HTTP client.
    /// </summary>
    string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the user agent string to be sent with HTTP requests.
    /// </summary>
    string UserAgent { get; set; }
    
    /// <summary>
    /// Gets or sets the task identifier for tracking HTTP operations.
    /// </summary>
    string TaskID { get; set; }
}
