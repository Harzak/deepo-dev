using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Spotify;

[Serializable]
public sealed class ResultSpotify<T>
{
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    [JsonPropertyName("items")]
    public IEnumerable<T>? Items { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    [JsonPropertyName("next")]
    public string? Next { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("previous")]
    public string? Previous { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

}
