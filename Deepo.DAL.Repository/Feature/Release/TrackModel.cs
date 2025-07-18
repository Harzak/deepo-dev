namespace Deepo.DAL.Repository.Feature.Release;

/// <summary>
/// Represents a track within an album
/// </summary>
public class TrackModel
{
    public int Position { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;
}