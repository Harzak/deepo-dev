using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Spotify;

[Serializable]
public sealed class DtoSpotifyAlbums
{
    [JsonPropertyName("albums")]
    public ResultSpotify<DtoSpotifyAlbum>? Result { get; set; }
}
