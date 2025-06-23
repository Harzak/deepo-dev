namespace Deepo.Dto;

[Serializable]
public class TrackVinyl
{
    public int Position { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
}