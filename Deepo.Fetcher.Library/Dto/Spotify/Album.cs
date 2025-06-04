using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Spotify;

[Serializable]
public sealed class Album
{
    [JsonPropertyName("album_type")]
    public string? AlbumType { get; set; }

    [JsonPropertyName("artists")]
    public IEnumerable<Artist>? Artists { get; set; }

    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }

    [JsonPropertyName("release_date_precision")]
    public string? ReleaseDatePrecision { get; set; }

    [JsonPropertyName("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}
