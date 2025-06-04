using Microsoft.Extensions.Options;
using System.Reflection;

namespace Deepo.Fetcher.Library.Configuration.Setting;

public class AppOptions : IOptions<AppOptions>
{
    public string ApplicationVersion { get; private set; }
    public string ApplicationName { get; private set; }

    public AppOptions Value => this;

    public AppOptions()
    {
        ApplicationVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "0.0.0.0";
        ApplicationName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? string.Empty;
    }
}

