using Deepo.Fetcher.Library.Configuration.Setting;
using Framework.Common.Utils.Extension;
using Framework.Common.Utils.Result;
using Framework.Web.Http.Client.Model;
using Framework.Web.Http.Client.Service;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TimeProvider = Framework.Common.Utils.Time.Provider.TimeProvider;

namespace Deepo.Fetcher.Library.Service.Spotify;

internal class SpotifyAuthService : HttpService, IAuthenticationHttpService
{
    private readonly HttpServiceOption _options;

    internal SpotifyAuthService(IHttpClientFactory httpClientFactory, IOptions<HttpServicesOption> options, ILogger<HttpService> logger)
    : base(httpClientFactory, new TimeProvider(), options.Value.SpotifyAuth, logger)
    {
        ArgumentNullException.ThrowIfNull(options?.Value?.Spotify, nameof(options.Value.Spotify));

        _options = options.Value.Spotify;

        base.SetAuthorization("Basic", $"{Convert.ToBase64String(Encoding.UTF8.GetBytes(_options.ConsumerKey + ":" + _options.ConsumerSecret))}");
    }

    public async Task<TokenModel?> RefreshTokenAsync(CancellationToken cancellationToken)
    {
        var kvp = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("grant_type", "client_credentials") };

        using (var content = new FormUrlEncodedContent(kvp))
        {
            OperationResult<string> response = await base.PostAsync("api/token", content, cancellationToken).ConfigureAwait(false);

            if (response != null && response.IsSuccess && response.HasContent)
            {
                SpotifyToken? spotifyToken = JsonSerializer.Deserialize<SpotifyToken>(response.Content);

                if (spotifyToken != null)
                {
                    return new TokenModel(spotifyToken.AccessToken, spotifyToken.TokenType, TimeSpan.FromSeconds(spotifyToken.ExpiresIn));
                }
            }
        }
        return null;
    }
}


[Serializable]
public class SpotifyToken
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}