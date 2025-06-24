using System.Text.Json.Serialization;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class DtoReleases
{
    [JsonPropertyName("pagination")]
    public DtoPagination? Pagination { get; set; }

    [JsonPropertyName("releases")]
    public IEnumerable<DtoRelease>? Items { get; set; }
}

