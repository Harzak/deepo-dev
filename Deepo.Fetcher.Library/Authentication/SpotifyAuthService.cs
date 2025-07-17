using Deepo.Fetcher.Library.Configuration.Setting;
using Deepo.Framework.Interfaces;
using Deepo.Framework.Results;
using Deepo.Framework.Time;
using Deepo.Framework.Web.Model;
using Deepo.Framework.Web.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Authentication;

/// <summary>
/// Spotify-specific implementation of the authentication HTTP service.
/// Handles OAuth2 client credentials flow for Spotify API authentication.
/// </summary>
internal class SpotifyAuthService : HttpService, IAuthenticationHttpService
{
    private readonly HttpServiceOption _options;

    internal SpotifyAuthService(
        IHttpClientFactory httpClientFactory,
        IOptions<HttpServicesOption> options,
        ILogger<HttpService> logger)
    : base(httpClientFactory,
        new DateTimeFacade(),
        options.Value.SpotifyAuth ?? throw new ArgumentNullException("options.Value.SpotifyAuth"),
        logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Spotify, nameof(options.Value.Spotify));

        _options = options.Value.Spotify;

        base.SetAuthorization("Basic", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(_options.ConsumerKey + ":" + _options.ConsumerSecret))}");
    }

    /// <summary>
    /// Refreshes the authentication token using the OAuth2 client credentials flow.
    /// </summary>
    /// <returns>A token model containing the access token and expiration information, or null if the request fails.</returns>
    public async Task<TokenModel?> RefreshTokenAsync(CancellationToken cancellationToken)
    {
        List<KeyValuePair<string, string>> kvp = [new("grant_type", "client_credentials")];

        using var content = new FormUrlEncodedContent(kvp);

        OperationResult<string> response = await base.PostAsync("api/token", content, cancellationToken).ConfigureAwait(false);

        if (response != null && response.IsSuccess && response.HasContent)
        {
            SpotifyToken? spotifyToken = JsonSerializer.Deserialize<SpotifyToken>(response.Content);

            if (spotifyToken?.AccessToken != null && spotifyToken?.TokenType != null)
            {
                return new TokenModel(spotifyToken.AccessToken, spotifyToken.TokenType, TimeSpan.FromSeconds(spotifyToken.ExpiresIn));
            }
        }
        return null;
    }
}

/// <summary>
/// Data transfer object representing a Spotify OAuth2 access token response.
/// Contains the access token, token type, and expiration information.
/// </summary>
[Serializable]
public class SpotifyToken
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the type of token (typically "Bearer").
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}