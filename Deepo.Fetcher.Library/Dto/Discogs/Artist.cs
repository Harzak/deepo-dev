using Deepo.DAL.Service.Interfaces;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class Artist : IAuthor
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("anv")]
    public string? Anv { get; set; }

    [JsonPropertyName("join")]
    public string? Join { get; set; }

    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("tracks")]
    public string? Tracks { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; set; }

    [JsonPropertyName("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    string IAuthor.Provider_Code => "DISCOGS";

    string IAuthor.Provider_Identifier => Id.ToString(CultureInfo.CurrentCulture);
}

