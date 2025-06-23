using System.Collections.ObjectModel;

namespace Deepo.Client.Web.Dto;

[Serializable]
public class ReleaseVinylExDto : ReleaseVinylDto
{
    public string Label { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public Collection<ReleaseVinylTrack> Tracklist { get; } = new Collection<ReleaseVinylTrack>([]);
    public Dictionary<string, string> Genres { get; } = [];
}