namespace Deepo.Client.Web.Dto;

[Serializable]
public class ReleaseVinylTrack
{
    public int Position { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
}