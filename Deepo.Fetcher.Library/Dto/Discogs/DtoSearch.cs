using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

[Serializable]
public sealed class AlbumSearch
{
    [JsonPropertyName("pagination")]
    public DtoPagination? Pagination { get; set; }

    [JsonPropertyName("results")]
    public IEnumerable<DtoAlbum>? Results { get; set; }
}

