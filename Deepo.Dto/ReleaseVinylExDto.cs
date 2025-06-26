using System.Collections.ObjectModel;

namespace Deepo.Dto;

[Serializable]
public class ReleaseVinylExDto : ReleaseVinylDto
{
    public string Label { get; set; } = string.Empty;
    public string? Duration { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Collection<TrackVinyl> Tracklist { get; } = new Collection<TrackVinyl>([]);
}