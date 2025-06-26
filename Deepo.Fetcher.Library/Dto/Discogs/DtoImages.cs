using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

[Serializable]
public sealed class DtoImages
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; set; }

    [JsonPropertyName("uri150")]
    public string? Uri150 { get; set; }

    [JsonPropertyName("width")]
    public string? Width { get; set; }

    [JsonPropertyName("height")]
    public string? Height { get; set; }
}

