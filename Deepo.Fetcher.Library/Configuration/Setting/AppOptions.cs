using Microsoft.Extensions.Options;
using System.Reflection;

namespace Deepo.Fetcher.Library.Configuration.Setting;

/// <summary>
/// Application-level configuration options that provide metadata about the running application.
/// Implements <see cref="IOptions{AppOptions}"/> for dependency injection integration.
/// </summary>
public class AppOptions : IOptions<AppOptions>
{
    /// <summary>
    /// Gets the version of the application, retrieved from the entry assembly.
    /// </summary>
    public string ApplicationVersion { get; private set; }
    
    /// <summary>
    /// Gets the name of the application, retrieved from the entry assembly.
    /// </summary>
    public string ApplicationName { get; private set; }

    /// <summary>
    /// Gets the current instance of <see cref="AppOptions"/> for the options pattern.
    /// </summary>
    public AppOptions Value => this;

    public AppOptions()
    {
        ApplicationVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.0.0";
        ApplicationName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
    }
}

