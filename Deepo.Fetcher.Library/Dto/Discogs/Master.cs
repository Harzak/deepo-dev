using Deepo.DAL.Service.Feature.Author;
using Deepo.DAL.Service.Feature.ReleaseAlbum;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class Master 
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

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("resource_url")]
    public string? ResourceUrl { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("artists")]
    public IEnumerable<Artist>? Artists { get; set; }

    [JsonPropertyName("artists_sort")]
    public string? ArtistsSort { get; set; }

    [JsonPropertyName("labels")]
    public IEnumerable<Label>? Labels { get; set; }

    [JsonPropertyName("companies")]
    public IEnumerable<Company>? Companies { get; set; }

    [JsonPropertyName("formats")]
    public IEnumerable<Format>? Formats { get; set; }

    [JsonPropertyName("data_quality")]
    public string? DataQuality { get; set; }

    [JsonPropertyName("format_quantity")]
    public int FormatQuantity { get; set; }

    [JsonPropertyName("date_added")]
    public DateTime DateAdded { get; set; }

    [JsonPropertyName("date_changed")]
    public DateTime DateChanged { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("released")]
    public string? Released { get; set; }

    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    [JsonPropertyName("released_formatted")]
    public string? ReleasedFormatted { get; set; }

    [JsonPropertyName("genres")]
    public IEnumerable<string>? Genres { get; set; }


    [JsonPropertyName("styles")]
    public IEnumerable<string>? Styles { get; set; }

    [JsonPropertyName("tracklist")]
    public IEnumerable<Tracklist>? Tracklist { get; set; }

    [JsonPropertyName("images")]
    public IEnumerable<Image>? Images { get; set; }

    [JsonPropertyName("thumb")]
    public string? ThumbsURL { get; set; }

    public Dictionary<string, string> ProvidersIdentifier { get; }


    public Master()
    {
        ProvidersIdentifier = [];
    }
}

