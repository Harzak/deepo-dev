using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Spotify;

[Serializable]
public sealed class Albums
{
    [JsonPropertyName("albums")]
    public ResultSpotify<Album>? Result { get; set; }
}
