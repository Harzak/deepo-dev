using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Fetcher.Library.Interfaces;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Web.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Deepo.Fetcher.Library.Authentication;

internal class AuthServiceFactory : IAuthServiceFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<HttpServicesOption> _options;
    private readonly ILoggerFactory _loggerFactory;

    public AuthServiceFactory(
        IHttpClientFactory httpClientFactory,
        IOptions<HttpServicesOption> options,
        ILoggerFactory loggerFactory)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
        _loggerFactory = loggerFactory;
    }

    public IAuthenticationHttpService CreateSpotifyAuthService()
    {
        return new SpotifyAuthService(_httpClientFactory, _options, _loggerFactory.CreateLogger<HttpService>());
    }
}