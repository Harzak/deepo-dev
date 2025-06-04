using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class Company
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("catno")]
    public string? Catno { get; set; }
}

