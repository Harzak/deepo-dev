using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

public interface IAuthServiceFactory
{
    IAuthenticationHttpService CreateSpotifyAuthService();
}