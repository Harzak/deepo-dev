using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

[Serializable]
public sealed class DtoDiscogsFormat
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("qty")]
    public string? Qty { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("descriptions")]
    public IEnumerable<string>? Descriptions { get; set; }
}

