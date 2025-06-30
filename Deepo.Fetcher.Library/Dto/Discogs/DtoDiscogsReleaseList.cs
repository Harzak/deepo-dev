using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class DtoDiscogsReleaseList
{
    [JsonPropertyName("pagination")]
    public DtoDiscogsPagination? Pagination { get; set; }

    [JsonPropertyName("releases")]
    public IEnumerable<DtoDiscogsRelease>? Items { get; set; }
}

