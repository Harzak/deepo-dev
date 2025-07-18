using Deepo.Framework.Interfaces;
using System.Reflection;

namespace Deepo.Fetcher.Library.Configuration.Setting;

/// <summary>
/// Configuration options for HTTP service clients.
/// </summary>
public class HttpServiceOption : IHttpClientOption
{
    public Uri BaseAddress { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user agent string for HTTP requests. Defaults to the entry assembly name.
    /// </summary>
    public string UserAgent { get; set; } = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
    
    /// <summary>
    /// Gets or sets the provider code for the HTTP service.
    /// </summary>
    public string ProviderCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the consumer key for API authentication.
    /// </summary>
    public string ConsumerKey { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the consumer secret for API authentication.
    /// </summary>
    public string ConsumerSecret { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the authentication token for API requests.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the task identifier for the HTTP service.
    /// </summary>
    public string TaskID { get; set; } = string.Empty;

    public HttpServiceOption()
    {

    }
}

