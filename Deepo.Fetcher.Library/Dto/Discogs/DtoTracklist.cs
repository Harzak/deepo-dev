using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Deepo.Fetcher.Library.Dto.Discogs;

public sealed class DtoTracklist
{
    [JsonPropertyName("position")]
    public string? Position { get; set; }

    [JsonPropertyName("type_")]
    public string? Type { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("duration")]
    public string? Duration { get; set; }
}

