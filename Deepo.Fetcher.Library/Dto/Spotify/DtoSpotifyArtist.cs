using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Spotify;

[Serializable]
public sealed class DtoSpotifyArtist
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}

