using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

[Serializable]
public sealed class DtoDiscogsAlbum
{
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("year")]
    public string? Year { get; set; }

    [JsonPropertyName("format")]
    public IEnumerable<string>? Format { get; set; }

    [JsonPropertyName("label")]
    public IEnumerable<string>? Label { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("genre")]
    public IEnumerable<string>? Genre { get; set; }

    [JsonPropertyName("style")]
    public IEnumerable<string>? Style { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("barcode")]
    public IEnumerable<object>? Barcode { get; set; }

    [JsonPropertyName("master_id")]
    public int MasterId { get; set; }

    [JsonPropertyName("master_url")]
    public string? MasterUrl { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("catno")]
    public string? Catno { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("thumb")]
    public string? Thumb { get; set; }

    [JsonPropertyName("cover_image")]
    public string? CoverImage { get; set; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; set; }

    [JsonPropertyName("format_quantity")]
    public int FormatQuantity { get; set; }

    [JsonPropertyName("formats")]
    public IEnumerable<DtoDiscogsFormat>? Formats { get; set; }
}

