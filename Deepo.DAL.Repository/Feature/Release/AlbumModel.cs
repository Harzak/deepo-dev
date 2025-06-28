using Deepo.DAL.Repository.Interfaces;

namespace Deepo.DAL.Repository.Feature.Release;

public class AlbumModel
{
    public string? Title { get; set; }
    public DateTime? ReleaseDateUTC { get; set; }
    public string? Description { get; set; }
    public string? ThumbURL { get; set; }
    public string? CoverURL { get; set; }
    public string? Label { get; set; }
    public string? Country { get; set; }
    public string? Market { get; set; }
    public double Duration { get; set; }
    public IEnumerable<IAuthor>? Artists { get; set; }
    public IEnumerable<string> Genres { get; set; }
    public IEnumerable<TrackModel> Tracklist { get; set; }
    public Dictionary<string, string> ProvidersIdentifier { get; }

    public AlbumModel()
    {
        this.ProvidersIdentifier = [];
        this.Genres = [];
        this.Tracklist = [];
    }
}

