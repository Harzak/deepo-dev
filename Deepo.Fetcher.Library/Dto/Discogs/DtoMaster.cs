using Deepo.DAL.Repository.Feature.Author;
using Deepo.DAL.Repository.Feature.Release;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class DtoMaster 
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
    public IEnumerable<DtoArtist>? Artists { get; set; }

    [JsonPropertyName("artists_sort")]
    public string? ArtistsSort { get; set; }

    [JsonPropertyName("labels")]
    public IEnumerable<DtoLabel>? Labels { get; set; }

    [JsonPropertyName("companies")]
    public IEnumerable<DtoCompany>? Companies { get; set; }

    [JsonPropertyName("formats")]
    public IEnumerable<DtoFormat>? Formats { get; set; }

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
    public IEnumerable<DtoTracklist>? Tracklist { get; set; }

    [JsonPropertyName("images")]
    public IEnumerable<DtoImage>? Images { get; set; }

    [JsonPropertyName("thumb")]
    public string? ThumbsURL { get; set; }

    public Dictionary<string, string> ProvidersIdentifier { get; }


    public DtoMaster()
    {
        ProvidersIdentifier = [];
    }
}

