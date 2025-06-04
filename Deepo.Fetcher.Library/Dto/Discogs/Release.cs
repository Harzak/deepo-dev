using Deepo.DAL.Service.Feature.Author;
using Deepo.DAL.Service.Feature.ReleaseAlbum;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

    public sealed class Release 
{
    private int _id;
    [JsonPropertyName("id")]
    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            ProvidersIdentifier.Add("DISCOGS", _id.ToString(CultureInfo.CurrentCulture));
        }
    }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; set; }

    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("artist")]
    public string? Artist { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("thumb")]
    public string? ThumbsURL { get; set; }

    [JsonPropertyName("main_release")]
    public int? MainRelease { get; set; }

    [JsonPropertyName("trackinfo")]
    public string? Trackinfo { get; set; }

    public Dictionary<string, string> ProvidersIdentifier { get; }

    public Release()
    {
        ProvidersIdentifier = [];
    }
}

