using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

[Serializable]
public sealed class AlbumSearch
{
    [JsonPropertyName("pagination")]
    public DtoDiscogsPagination? Pagination { get; set; }

    [JsonPropertyName("results")]
    public IEnumerable<DtoDiscogsAlbum>? Results { get; set; }
}

