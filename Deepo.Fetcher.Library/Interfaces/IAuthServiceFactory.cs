using Deepo.Framework.Interfaces;

namespace Deepo.Fetcher.Library.Interfaces;

/// <summary>
/// Factory interface for creating authentication service instances.
/// Provides a centralized way to create authentication services for different providers.
/// </summary>
public interface IAuthServiceFactory
{
    /// <summary>
    /// Creates an authentication service instance specifically for Spotify API authentication.
    /// </summary>
    IAuthenticationHttpService CreateSpotifyAuthService();
}